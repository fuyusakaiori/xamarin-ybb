using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 警告服务。
    /// </summary>
    public class AlertService : IAlertService {
        /// <summary>
        /// 显示警告。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="message">信息。</param>
        /// <param name="button">按钮文字。</param>
        public void ShowAlert(string title, string message, string button) =>
            (Application.Current as App).MainPage.DisplayAlert(title, message,
                button);
    }
}