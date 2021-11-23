using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MasterDetailTemplate.Services.Implementations {
    /// <summary>
    /// 缓存的内容页面激活服务。
    /// </summary>
    public class
        CachedContentPageActivationService : IContentPageActivationService {
        /******** 公开变量 ********/

        /******** 私有变量 ********/

        /// <summary>
        /// 页面缓存。
        /// </summary>
        private Dictionary<string, ContentPage> cache =
            new Dictionary<string, ContentPage>();

        /******** 继承方法 ********/

        /// <summary>
        /// 激活页面。
        /// </summary>
        /// <returns>页面键。</returns>
        /// <param name="pageKey">激活的根页面。</param>
        public ContentPage Activate(string pageKey) =>
            cache.ContainsKey(pageKey)
                ? cache[pageKey]
                : cache[pageKey] = (ContentPage) Activator.CreateInstance(
                    ContentNavigationServiceConstants.PageKeyTypeDictionary[
                        pageKey]);

        /******** 公开方法 ********/

        /******** 私有方法 ********/
    }
}