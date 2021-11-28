using MasterDetailTemplate.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public class QuestionService : IQuestionService
    {
        // 数据库连接
        private const string DbName = "errorbook.sqlite3";
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), DbName);

        // 控制数据库连接的对象
        private SQLiteAsyncConnection _connection;
        public SQLiteAsyncConnection Connection =>
            _connection ?? (_connection = new SQLiteAsyncConnection(DbPath));


        private IPreferenceStorage _preferenceStorage;

        public QuestionService(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }

        public const int Version = 1;

        /// <summary>
        /// 数据库版本键。
        /// </summary>
        public const string VersionKey =
            nameof(QuestionService) + "." + nameof(Version);

        public async Task InitializeAsync()
        {
            // 1. 在存储本地数据的地方创建数据库文件
            using (FileStream fs = new FileStream(DbPath, FileMode.Create))
            // 2. 读取嵌入式数据库资源并且将其写入到数据库文件中: TODO 前提是嵌入式资源必须被配置进去了, 否则就是空指针异常   
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DbName))
            {
                await stream.CopyToAsync(fs);
            }

            _preferenceStorage.Set(VersionKey, Version);
        }

        public bool Initialized() =>
            _preferenceStorage.Get(VersionKey, -1) == Version;

        // 创建错题
        public async Task CreateQuestion(Question question)
        {
            await Connection.InsertAsync(question);
        }

        // 删除错题
        public async Task DeleteQuestion(int id)
        {
            await Connection.Table<Question>()
                .DeleteAsync(question => question.Id == id);
        }


        // 查询错题
         public async Task<Question> GetQuestion(int id)
        {
            return await Connection.Table<Question>().
                Where(question => question.Id == id).FirstOrDefaultAsync();
        }

       // 获取错题集合
        public async Task<IList<Question>> GetQuestionList(
            Expression<Func<Question, bool>> @where, int skip, int take)
        {
            List<Question> list = await Connection.Table<Question>().
                Where(@where).Skip(skip).Take(take).ToListAsync();
            return list;
        }

        public async Task UpdateQuestion(Question question)
        {
            System.Diagnostics.Debug.WriteLine("===============业务层===============");
            System.Diagnostics.Debug.WriteLine(question.Id + "\t" + question.Name + "\t" + question.Content);
            await Connection.UpdateAsync(question);
        }
        public void CloseConnection() => Connection.CloseAsync();
    }
}
