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

        [SQLite.Column("path")]
        public string Path { get; set; }

        [SQLite.Column("category_id")]
        public int CategoryId { get; set; }

        [SQLite.Column("category_name")]
        public string CategoryName { get; set; }
    }
}
