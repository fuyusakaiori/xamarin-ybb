using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MasterDetailTemplate.Models;
using SQLite;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 诗词存储接口。
    /// </summary>
    public interface IPoetryStorage {
        /// <summary>
        /// 初始化。
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 是否已经初始化。
        /// </summary>
        bool Initialized();

        /// <summary>
        /// 获取一个诗词。
        /// </summary>
        /// <param name="id">诗词id。</param>
        Task<Poetry> GetPoetryAsync(int id);

        /// <summary>
        /// 获取满足给定条件的诗词集合。
        /// </summary>
        /// <param name="where">Where条件。</param>
        /// <param name="skip">跳过数量。</param>
        /// <param name="take">获取数量。</param>
        Task<IList<Poetry>> GetPoetriesAsync(
            Expression<Func<Poetry, bool>> where, int skip, int take);
    }
}