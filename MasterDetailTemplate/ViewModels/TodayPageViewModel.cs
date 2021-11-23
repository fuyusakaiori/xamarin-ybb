using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;

namespace MasterDetailTemplate.ViewModels {
    /// <summary>
    /// 今日推荐页ViewModel
    /// </summary>
    public class TodayPageViewModel : ViewModelBase {
        /******** 构造函数 ********/

        /// <summary>
        /// 今日图片服务。
        /// </summary>
        private ITodayImageService _todayImageService;

        /// <summary>
        /// 今日诗词服务。
        /// </summary>
        private ITodayPoetryService _todayPoetryService;

        // /// <summary>
        // /// 浏览器服务。
        // /// </summary>
        // private IBrowserService _browserService;
        //
        // /// <summary>
        // /// 内容导航服务。
        // /// </summary>
        // private IContentNavigationService _contentNavigationService;
        //
        // /// <summary>
        // /// 根导航服务。
        // /// </summary>
        // private IRootNavigationService _rootNavigationService;

        /// <summary>
        /// 今日推荐页ViewModel
        /// </summary>
        /// <param name="todayImageService">今日图片服务。</param>
        /// <param name="todayPoetryService">今日诗词服务。</param>
        /// <param name="browserService">浏览器服务。</param>
        /// <param name="contentNavigationService">内容导航服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        public TodayPageViewModel(ITodayImageService todayImageService,
            ITodayPoetryService todayPoetryService) {
            _todayImageService = todayImageService;
            _todayPoetryService = todayPoetryService;
            // _browserService = browserService;
            // _contentNavigationService = contentNavigationService;
            // _rootNavigationService = rootNavigationService;
        }

        /******** 绑定属性 ********/

        /// <summary>
        /// 今日图片。
        /// </summary>
        public TodayImage TodayImage {
            get => _todayImage;
            set => Set(nameof(TodayImage), ref _todayImage, value);
        }

        /// <summary>
        /// 今日图片。
        /// </summary>
        private TodayImage _todayImage;

        /// <summary>
        /// 今日诗词。
        /// </summary>
        public TodayPoetry TodayPoetry {
            get => _todayPoetry;
            set => Set(nameof(TodayPoetry), ref _todayPoetry, value);
        }

        /// <summary>
        /// 今日诗词。
        /// </summary>
        private TodayPoetry _todayPoetry;

        /// <summary>
        /// 今日诗词已加载。
        /// </summary>
        public bool TodayPoetryLoaded {
            get => _todayPoetryLoaded;
            set =>
                Set(nameof(TodayPoetryLoaded), ref _todayPoetryLoaded, value);
        }

        /// <summary>
        /// 今日诗词已加载。
        /// </summary>
        private bool _todayPoetryLoaded;

        /// <summary>
        /// 正在加载今日诗词。
        /// </summary>
        public bool TodayPoetryLoading {
            get => _todayPoetryLoading;
            set =>
                Set(nameof(TodayPoetryLoading), ref _todayPoetryLoading, value);
        }

        /// <summary>
        /// 正在加载今日诗词。
        /// </summary>
        private bool _todayPoetryLoading;

        /******** 绑定命令 ********/

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(PageAppearingCommandFunction));

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        internal void PageAppearingCommandFunction() {
            Task.Run(async () => {
                TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            });

            Task.Run(async () => {
                TodayImage = await _todayImageService.GetTodayImageAsync();
            });
        }

        /// <summary>
        /// 查看详细命令。
        /// </summary>
        public RelayCommand ShowDetailCommand =>
            _showDetailCommand ?? (_showDetailCommand =
                new RelayCommand(async () =>
                    await ShowDetailCommandFunction()));

        /// <summary>
        /// 查看详细命令。
        /// </summary>
        private RelayCommand _showDetailCommand;

        internal async Task ShowDetailCommandFunction() =>
            throw new NotImplementedException();

        /// <summary>
        /// 今日诗词命令。
        /// </summary>
        public RelayCommand JinrishiciCommand =>
            _jinrishiciCommand ?? (_jinrishiciCommand =
                new RelayCommand(async () =>
                    await JinrishiciCommandFunction()));

        /// <summary>
        /// 今日诗词命令。
        /// </summary>
        private RelayCommand _jinrishiciCommand;

        internal async Task JinrishiciCommandFunction() =>
            throw new NotImplementedException();

        /// <summary>
        /// 版权信息命令。
        /// </summary>
        public RelayCommand CopyrightCommand =>
            _copyrightCommand ?? (_copyrightCommand =
                new RelayCommand(async () => await CopyrightCommandFunction()));

        /// <summary>
        /// 版权信息命令。
        /// </summary>
        private RelayCommand _copyrightCommand;

        internal async Task CopyrightCommandFunction() =>
            throw new NotImplementedException();

        /// <summary>
        /// 搜索命令。
        /// </summary>
        public RelayCommand QueryCommand =>
            _queryCommand ?? (_queryCommand = new RelayCommand(async () =>
                await QueryCommandFunction()));

        /// <summary>
        /// 搜索命令。
        /// </summary>
        private RelayCommand _queryCommand;

        internal async Task QueryCommandFunction() {
            throw new NotImplementedException();
        }
    }
}