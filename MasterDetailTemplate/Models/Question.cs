using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDetailTemplate.Models
{
    [SQLite.Table("error_question")]
    public  class Question {
        [SQLite.PrimaryKey, SQLite.Column("id"),SQLite.AutoIncrement]
        public int Id { get; set; }

        [SQLite.Column("name")]
        public string Name { get; set; }

        [SQLite.Column("content")]
        public string Content { get; set; }

        // TODO 错题类别和错题图片暂时没有
    }
}
