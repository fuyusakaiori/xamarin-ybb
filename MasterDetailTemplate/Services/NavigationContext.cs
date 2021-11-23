using Xamarin.Forms;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 导航参数。
    /// </summary>
    public static class NavigationContext {
        /// <summary>
        /// 导航参数属性。
        /// </summary>
        public static readonly BindableProperty NavigationParameterProperty =
            BindableProperty.CreateAttached("NavigationParameter",
                typeof(object), typeof(NavigationContext), null,
                BindingMode.OneWayToSource);

        /// <summary>
        /// 设置导航参数。
        /// </summary>
        /// <param name="page">页面。</param>
        /// <param name="value">导航参数。</param>
        public static void SetParameter(BindableObject page, object value) =>
            page.SetValue(NavigationParameterProperty, value);
    }
}