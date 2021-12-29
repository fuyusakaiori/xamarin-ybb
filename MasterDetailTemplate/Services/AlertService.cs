using System.Threading.Tasks;
using MasterDetailTemplate.Views;
using Xamarin.Forms;

namespace MasterDetailTemplate.Services
{
    /// <summary>
    /// 警告服务。
    /// </summary>
    public class AlertService : IAlertService {
        /******** 公开变量 ********/

        /******** 私有变量 ********/

        /// <summary>
        /// 用于显示警告的MainPage。
        /// </summary>
        private MainPage MainPage => _mainPage ??
            (_mainPage = Application.Current.MainPage as MainPage);

        /// <summary>
        /// 用于显示警告的MainPage。
        /// </summary>
        private MainPage _mainPage;

        /******** 继承方法 ********/

        /// <summary>
        /// 显示警告。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="message">信息。</param>
        /// <param name="button">按钮文字。</param>
        public void ShowAlert(string title, string message, string button) =>
            Device.BeginInvokeOnMainThread(() =>
                MainPage.DisplayAlert(title, message, button));

        /******** 公开方法 ********/

        /******** 私有方法 ********/
    }
}