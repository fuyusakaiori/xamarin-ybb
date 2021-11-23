namespace MasterDetailTemplate.Models {
    /// <summary>
    /// 诗词类。
    /// </summary>
    [SQLite.Table("works")]
    public class Poetry {
        /// <summary>
        /// 主键。
        /// </summary>
        [SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [SQLite.Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 作者。
        /// </summary>
        [SQLite.Column("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 朝代。
        /// </summary>
        [SQLite.Column("dynasty")]
        public string Dynasty { get; set; }

        /// <summary>
        /// 正文。
        /// </summary>
        [SQLite.Column("content")]
        public string Content { get; set; }

        private string _snippet;

        /// <summary>
        /// 预览。
        /// </summary>
        [SQLite.Ignore]
        public string Snippet =>
            _snippet ?? (_snippet
                = Content.Split('。')[0]
                    .Replace("\r\n", " "));
    }
}