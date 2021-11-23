using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using Xamarin.Forms.Extended;

namespace MasterDetailTemplate.ViewModels {
    /// <summary>
    /// 搜索结果页ViewModel。
    /// </summary>
    public class ResultPageViewModel : ViewModelBase {
        /// <summary>
        /// 诗词集合。
        /// </summary>
        public InfiniteScrollCollection<Poetry> PoetryCollection { get; }

        /// <summary>
        /// 能否加载更多结果。
        /// </summary>
        private bool _canLoadMore;

        /// <summary>
        /// 诗词存储。
        /// </summary>
        private IPoetryStorage _poetryStorage;


        public ResultPageViewModel(IPoetryStorage poetryStorage) {
            _poetryStorage = poetryStorage;

            // TODO 删除供测试的代码。
            Where = Expression.Lambda<Func<Poetry, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));

            PoetryCollection = new InfiniteScrollCollection<Poetry>();
            PoetryCollection.OnCanLoadMore = () => _canLoadMore;
            PoetryCollection.OnLoadMore = async () => {
                Status = Loading;
                var poetries =
                    await poetryStorage.GetPoetriesAsync(Where,
                        PoetryCollection.Count, 20);
                Status = string.Empty;

                if (poetries.Count < 20) {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (PoetryCollection.Count == 0 && poetries.Count == 0) {
                    Status = NoResult;
                }

                return poetries;
            };
        }

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        public RelayCommand _pageAppearingCommand;

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction() {
            // TODO 删除供测试的代码。
            await _poetryStorage.InitializeAsync();

            PoetryCollection.Clear();
            _canLoadMore = true;
            await PoetryCollection.LoadMoreAsync();
        }

        /// <summary>
        /// 查询语句。
        /// </summary>
        public Expression<Func<Poetry, bool>> Where {
            get => _where;
            set { Set(nameof(Where), ref _where, value); }
        }

        /// <summary>
        /// 查询语句。
        /// </summary>
        private Expression<Func<Poetry, bool>> _where;

        /// <summary>
        /// 加载状态。
        /// </summary>
        public string Status {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        /// <summary>
        /// 加载状态。
        /// </summary>
        private string _status;

        /// <summary>
        /// 正在载入。
        /// </summary>
        public const string Loading = "正在载入";

        /// <summary>
        /// 没有满足条件的结果。
        /// </summary>
        public const string NoResult = "没有满足条件的结果";

        /// <summary>
        /// 没有更多结果。
        /// </summary>
        public const string NoMoreResult = "没有更多结果";
    }
}