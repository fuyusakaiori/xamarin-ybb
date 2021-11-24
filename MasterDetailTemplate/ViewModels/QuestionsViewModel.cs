using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using MasterDetailTemplate.Services.Implementations;
using Xamarin.Forms.Extended;

namespace MasterDetailTemplate.ViewModels
{
    /// <summary>
    /// 错题列表。
    /// </summary>
    public class QuestionsViewModel: ViewModelBase {

        // 用于判断是否开始加载的标志位
        private bool _canLoadMore;

        // 查询语句需要 Lambda 表达式, 所以需要单独提供一个字段
        private Expression<Func<Question, bool>> _where;

        public Expression<Func<Question, bool>> Where
        {
            get => _where;
            // 确定需要更新的属性名称, 然后获取属性的引用, 最后更新值
            set => Set(nameof(Where), ref _where, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        // 提示信息
        public const string Loading = "正在载入";

        public const string NoResult = "没有满足条件的结果";

        public const string NoMoreResult = "没有更多结果";

        // 错题集合
        public InfiniteScrollCollection<Question> QustionCollection { get; set; }

        // 导航服务
        private IContentNavigationService _contentNavigationService;

        private IQuestionService _questionService;


        public QuestionsViewModel(IQuestionService questionService) {
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 错题相关
            _questionService = questionService;
            // 初始化集合
            Where = Expression.Lambda<Func<Question, bool>>(
               // 1. 条件语句
               Expression.Constant(true),
               // 2. 返回的参数?
               Expression.Parameter(typeof(Question), "poetry"));
            QustionCollection = new InfiniteScrollCollection<Question>();
            QustionCollection.OnCanLoadMore = () => _canLoadMore;
            QustionCollection.OnLoadMore = async () => {
                // 开始加载
                IList<Question> list = await _questionService.GetQuestionList(Where,
                    QustionCollection.Count, 20);
                Status = string.Empty;
                if (list.Count < 20)
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (QustionCollection.Count == 0 && list.Count == 0)
                    Status = NoResult;

                return list;
            };

        }

        // 显示所有错题
        public RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction() {
            await _questionService.InitializeAsync();
            QustionCollection.Clear();
            _canLoadMore = true;
            await QustionCollection.LoadMoreAsync();
            _questionService.CloseConnection();
        }



        // 点击错题单元格后自动跳转到错题详情页
        private RelayCommand<Question> _questionTappedCommand;
        public RelayCommand<Question> QuestionTappedCommand =>
            _questionTappedCommand ?? (_questionTappedCommand =
                new RelayCommand<Question>(async question =>
                    await QuestionTappedCommandFunction(question)));


        internal async Task QuestionTappedCommandFunction(Question question) =>
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationServiceConstants.QuestionDetail, question);

    }
}
