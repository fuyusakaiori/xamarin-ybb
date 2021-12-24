using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDetailTemplate.Models
{
    [SQLite.Table("error_question_category")]
    public class QuestionCategory
    {
        [SQLite.PrimaryKey, SQLite.Column("id"), SQLite.AutoIncrement]
        public int Id { get; set; }

        [SQLite.Column("name")]
        public string Name { get; set; }
    }
}
