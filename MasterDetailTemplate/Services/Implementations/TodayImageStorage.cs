using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MasterDetailTemplate.Models;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 今日图片存储。
    /// </summary>
    public class TodayImageStorage : ITodayImageStorage {
        /******** 公开变量 ********/

        /// <summary>
        /// 完整开始日期配置项键。
        /// </summary>
        public static readonly string FullStartDateKey =
            nameof(TodayImage) + "." + nameof(TodayImage.FullStartDate);

        /// <summary>
        /// 过期时间配置项键。
        /// </summary>
        public static readonly string ExpiresAtKey =
            nameof(TodayImage) + "." + nameof(TodayImage.ExpiresAt);

        /// <summary>
        /// 版权信息配置项键。
        /// </summary>
        public static readonly string CopyrightKey =
            nameof(TodayImage) + "." + nameof(TodayImage.Copyright);

        /// <summary>
        /// 版权链接配置项键。
        /// </summary>
        public static readonly string CopyrightLinkKey =
            nameof(TodayImage) + "." + nameof(TodayImage.CopyrightLink);

        /// <summary>
        /// 完整开始日期默认值。
        /// </summary>
        public const string FullStartDateDefault = "201901010700";

        /// <summary>
        /// 过期时间默认值。
        /// </summary>
        public static readonly DateTime ExpiresAtDefault =
            new DateTime(2019, 1, 2, 7, 0, 0);

        /// <summary>
        /// 版权信息默认值。
        /// </summary>
        public const string CopyrightDefault =
            "Salt field province vietnam work (© Quangpraha/Pixabay)";

        /// <summary>
        /// 版权链接默认值。
        /// </summary>
        public const string CopyrightLinkDefault =
            "https://pixabay.com/photos/salt-field-province-vietnam-work-3344508/";

        /// <summary>
        /// 今日图片文件名。
        /// </summary>
        public const string FileName = "todayImage.bin";

        /// <summary>
        /// 今日图片文件路径。
        /// </summary>
        public static readonly string TodayImagePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), FileName);

        /******** 私有变量 ********/

        /// <summary>
        /// 偏好存储。
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        /******** 继承方法 ********/

        /// <summary>
        /// 获得今日图片。
        /// </summary>
        /// <param name="includingImageStream">包含图片流。</param>
        public async Task<TodayImage> GetTodayImageAsync(
            bool includingImageStream) {
            var todayImage = new TodayImage {
                FullStartDate =
                    _preferenceStorage.Get(FullStartDateKey,
                        FullStartDateDefault),
                ExpiresAt =
                    _preferenceStorage.Get(ExpiresAtKey, ExpiresAtDefault),
                Copyright =
                    _preferenceStorage.Get(CopyrightKey, CopyrightDefault),
                CopyrightLink = _preferenceStorage.Get(CopyrightLinkKey,
                    CopyrightLinkDefault)
            };

            if (!File.Exists(TodayImagePath)) {
                using (var imageFileStream =
                    new FileStream(TodayImagePath, FileMode.Create))
                using (var imageAssetStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(FileName)) {
                    await imageAssetStream.CopyToAsync(imageFileStream);
                }
            }

            if (!includingImageStream)
                return todayImage;

            var imageMemoryStream = new MemoryStream();
            using (var imageFileStream =
                new FileStream(TodayImagePath, FileMode.Open)) {
                await imageFileStream.CopyToAsync(imageMemoryStream);
            }

            todayImage.ImageBytes = imageMemoryStream.ToArray();


            return todayImage;
        }

        /// <summary>
        /// 保存今日图片。
        /// </summary>
        /// <param name="todayImage">待更新的今日图片。</param>
        public async Task SaveTodayImageAsync(TodayImage todayImage) {
            _preferenceStorage.Set(ExpiresAtKey, todayImage.ExpiresAt);
            if (todayImage.Scope == TodayImage.ModificationScope.ExpiresAt)
                return;

            _preferenceStorage.Set(FullStartDateKey, todayImage.FullStartDate);
            _preferenceStorage.Set(CopyrightKey, todayImage.Copyright);
            _preferenceStorage.Set(CopyrightLinkKey, todayImage.CopyrightLink);

            using (var imageFileStream =
                new FileStream(TodayImagePath, FileMode.Create)) {
                await imageFileStream.WriteAsync(todayImage.ImageBytes, 0,
                    todayImage.ImageBytes.Length);
            }
        }

        /******** 公开方法 ********/

        /// <summary>
        /// 今日图片存储。
        /// </summary>
        /// <param name="preferenceStorage">偏好存储。</param>
        public TodayImageStorage(IPreferenceStorage preferenceStorage) {
            _preferenceStorage = preferenceStorage;
        }

        /******** 私有方法 ********/
    }
}