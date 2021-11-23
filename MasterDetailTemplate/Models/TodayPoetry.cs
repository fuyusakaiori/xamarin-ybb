using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDetailTemplate.Models {
    /// <summary>
    /// 今日诗词。
    /// </summary>
    public class TodayPoetry {
        /// <summary>
        /// 预览。
        /// </summary>
        public string Snippet { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        /// <value>The title.</value>
        public string Name { get; set; }

        /// <summary>
        /// 朝代。
        /// </summary>
        public string Dynasty { get; set; }

        /// <summary>
        /// 作者。
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 正文。
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// 译文。
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// 来源。
        /// </summary>
        public string Source { get; set; }
    }
}