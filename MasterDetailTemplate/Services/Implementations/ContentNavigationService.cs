using System;
using System.Threading.Tasks;
using MasterDetailTemplate.Views;
using Xamarin.Forms;

namespace MasterDetailTemplate.Services.Implementations {
    /// <summary>
    /// 内容导航服务。
    /// </summary>
    public class ContentNavigationService : IContentNavigationService {
        /******** 公开变量 ********/

        /******** 私有变量 ********/

        /// <summary>
        /// 内容页面激活服务。
        /// </summary>
        private IContentPageActivationService contentPageActivationService;

        /// <summary>
        /// 导航工具。
        /// </summary>
        private MainPage MainPage =>
            _mainPage ?? (_mainPage = Application.Current.MainPage as MainPage);

        private MainPage _mainPage;

        /******** 继承方法 ********/

        /// <summary>
        /// 导航到页面。
        /// </summary>
        /// <param name="pageKey">页面键。</param>
        public async Task NavigateToAsync(string pageKey) {
            await MainPage.Detail.Navigation.PushAsync(
                contentPageActivationService.Activate(pageKey));
        }

        /// <summary>
        /// 导航到页面。
        /// </summary>
        /// <param name="pageKey">页面键。</param>
        /// <param name="parameter">参数。</param>
        public async Task NavigateToAsync(string pageKey, object parameter) {
            var page = contentPageActivationService.Activate(pageKey);
            NavigationContext.SetParameter(page, parameter);
            await MainPage.Detail.Navigation.PushAsync(page);
        }

        /******** 公开方法 ********/

        public ContentNavigationService(
            IContentPageActivationService contentPageActivationService) {
            this.contentPageActivationService = contentPageActivationService;
        }

        /******** 私有方法 ********/
    }
}