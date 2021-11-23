using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MasterDetailTemplate.Views;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 内容导航服务
    /// </summary>
    /// <seealso cref="GalaSoft.MvvmLight.Views.INavigationService"/>
    public interface IContentNavigationService {
        /// <summary>
        /// Instructs the navigation service to display a new page
        /// corresponding to the given key. Depending on the platforms,
        /// the navigation service might have to be configured with a
        /// key/page list.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        Task NavigateToAsync(string pageKey);

        /// <summary>
        /// Instructs the navigation service to display a new page
        /// corresponding to the given key, and passes a parameter
        /// to the new page.
        /// Depending on the platforms, the navigation service might 
        /// have to be Configure with a key/page list.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        Task NavigateToAsync(string pageKey, object parameter);
    }

    /// <summary>
    /// 内容导航服务常量。
    /// </summary>
    public static class ContentNavigationServiceConstants {
        /// <summary>
        /// 搜索结果页。
        /// </summary>
        public static readonly string ResultPage = nameof(Views.ResultPage);

        /// <summary>
        /// 问题详情页。
        /// </summary>
        public static readonly string QuestionDetail = nameof(Views.QuestionDetail);

        /// <summary>
        /// 页面键-页面类型字典。
        /// </summary>
        public static readonly ReadOnlyDictionary<string, Type>
            PageKeyTypeDictionary = new ReadOnlyDictionary<string, Type>(
                new Dictionary<string, Type> {
                    [ResultPage] = typeof(ResultPage),
                    [QuestionDetail] = typeof(QuestionDetail)
                });
    }
}