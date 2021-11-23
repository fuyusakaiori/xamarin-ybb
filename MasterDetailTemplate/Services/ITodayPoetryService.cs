using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MasterDetailTemplate.Models;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 今日诗词服务。
    /// </summary>
    public interface ITodayPoetryService {
        /// <summary>
        /// 获得今日诗词。
        /// </summary>
        Task<TodayPoetry> GetTodayPoetryAsync();
    }

    public static class TodayPoetrySources {
        public const string JINRISHICI = nameof(JINRISHICI);
        public const string LOCAL = nameof(LOCAL);
    }
}