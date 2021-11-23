using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MasterDetailTemplate.Models;
using SQLite;
using Xamarin.Essentials;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 诗词存储。
    /// </summary>
    public class PoetryStorage : IPoetryStorage {
        /// <summary>
        /// 诗词存储。
        /// </summary>
        /// <param name="preferenceStorage">偏好存储。</param>
        public PoetryStorage(IPreferenceStorage preferenceStorage) {
            _preferenceStorage = preferenceStorage;
        }

        /// <summary>
        /// 偏好存储。
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        /// <summary>
        /// 数据库名。
        /// </summary>
        private const string DbName = "poetrydb.sqlite3";

        /// <summary>
        /// 诗词数据库路径。
        /// </summary>
        public static readonly string PoetryDbPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), DbName);

        /// <summary>
        /// 数据库版本。
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// 数据库版本键。
        /// </summary>
        public const string VersionKey =
            nameof(PoetryStorage) + "." + nameof(Version);

        /// <summary>
        /// 数据库连接影子变量。
        /// </summary>
        private SQLiteAsyncConnection _connection;

        /// <summary>
        /// 数据库连接。
        /// </summary>
        private SQLiteAsyncConnection Connection =>
            _connection ??
            (_connection = new SQLiteAsyncConnection(PoetryDbPath));


        /// <summary>
        /// 初始化。
        /// </summary>
        public async Task InitializeAsync() {
            using (var dbFileStream =
                new FileStream(PoetryDbPath, FileMode.Create))
            using (var dbAssertStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(DbName)) {
                await dbAssertStream.CopyToAsync(dbFileStream);
            }

            _preferenceStorage.Set(VersionKey, Version);
        }

        /// <summary>
        /// 是否已经初始化。
        /// </summary>
        public bool Initialized() =>
            _preferenceStorage.Get(VersionKey, -1) == Version;

        /// <summary>
        /// 获取一个诗词。
        /// </summary>
        /// <param name="id">诗词id。</param>
        public async Task<Poetry> GetPoetryAsync(int id) =>
            await Connection.Table<Poetry>().Where(p => p.Id == id)
                .FirstOrDefaultAsync();

        /// <summary>
        /// 获取满足给定条件的诗词集合。
        /// </summary>
        /// <param name="where">Where条件。</param>
        /// <param name="skip">跳过数量。</param>
        /// <param name="take">获取数量。</param>
        public async Task<IList<Poetry>> GetPoetriesAsync(
            Expression<Func<Poetry, bool>> @where, int skip, int take) =>
            await Connection.Table<Poetry>().Where(@where).Skip(skip).Take(take)
                .ToListAsync();
    }
}