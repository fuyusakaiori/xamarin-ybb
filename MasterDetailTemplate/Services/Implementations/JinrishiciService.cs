using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MasterDetailTemplate.Models;
using Newtonsoft.Json;

namespace MasterDetailTemplate.Services {
    /// <summary>
    /// 今日诗词服务。
    /// </summary>
    public class JinrishiciService : ITodayPoetryService {
        /// <summary>
        /// 今日诗词服务器。
        /// </summary>
        private const string Server = "今日诗词服务器";

        /// <summary>
        /// 今日诗词token键。
        /// </summary>
        public static readonly string JinrishiciTokenKey =
            nameof(JinrishiciService) + ".Token";

        /// <summary>
        /// 警告服务。
        /// </summary>
        private IAlertService _alertService;

        /// <summary>
        /// 偏好存储。
        /// </summary>
        private IPreferenceStorage _preferenceStorage;

        /// <summary>
        /// 诗词存储。
        /// </summary>
        private IPoetryStorage _poetryStorage;

        /// <summary>
        /// 今日诗词服务。
        /// </summary>
        /// <param name="alertService">警告服务。</param>
        /// <param name="poetryStorage">诗词存储。</param>
        /// <param name="preferenceStorage">偏好存储。</param>
        public JinrishiciService(IAlertService alertService,
            IPoetryStorage poetryStorage,
            IPreferenceStorage preferenceStorage) {
            _alertService = alertService;
            _poetryStorage = poetryStorage;
            _preferenceStorage = preferenceStorage;
        }

        /// <summary>
        /// 获得今日诗词。
        /// </summary>
        public async Task<TodayPoetry> GetTodayPoetryAsync() {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token)) {
                return await GetRandomPoetryAsync();
            }

            JinrishiciSentence jinrishiciSentence;
            using (var httpClient = new HttpClient()) {
                var headers = httpClient.DefaultRequestHeaders;
                headers.Add("X-User-Token", token);

                HttpResponseMessage response;
                try {
                    response =
                        await httpClient.GetAsync(
                            "https://v2.jinrishici.com/sentence");
                    response.EnsureSuccessStatusCode();
                } catch (Exception e) {
                    _alertService.ShowAlert(
                        ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                        ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                        ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                    return await GetRandomPoetryAsync();
                }

                var json = await response.Content.ReadAsStringAsync();
                jinrishiciSentence =
                    JsonConvert.DeserializeObject<JinrishiciSentence>(json);
            }

            return new TodayPoetry {
                Snippet = jinrishiciSentence.Data.Content,
                Name = jinrishiciSentence.Data.Origin.Title,
                Dynasty = jinrishiciSentence.Data.Origin.Dynasty,
                AuthorName = jinrishiciSentence.Data.Origin.Author,
                Content =
                    string.Join("\n", jinrishiciSentence.Data.Origin.Content),
                Translation = jinrishiciSentence.Data.Origin.Translate == null
                    ? ""
                    : string.Join("\n",
                        jinrishiciSentence.Data.Origin.Translate),
                Source = TodayPoetrySources.JINRISHICI
            };
        }

        public async Task<string> GetTokenAsync() {
            var token =
                _preferenceStorage.Get(JinrishiciTokenKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(token)) {
                return token;
            }

            using (var httpClient = new HttpClient()) {
                HttpResponseMessage response;
                try {
                    response =
                        await httpClient.GetAsync(
                            "https://v2.jinrishici.com/token");
                    response.EnsureSuccessStatusCode();
                } catch (Exception e) {
                    _alertService.ShowAlert(
                        ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                        ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                        ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                    return token;
                }

                var json = await response.Content.ReadAsStringAsync();
                var jinrishiciToken =
                    JsonConvert.DeserializeObject<JinrishiciToken>(json);
                token = jinrishiciToken.Data;
            }

            _preferenceStorage.Set(JinrishiciTokenKey, token);
            return token;
        }

        /// <summary>
        /// 获得随机诗词。
        /// </summary>
        internal async Task<TodayPoetry> GetRandomPoetryAsync() {
            var poetries = await _poetryStorage.GetPoetriesAsync(
                Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(Poetry), "p")),
                new Random().Next(30), 1);
            var poetry = poetries[0];
            return new TodayPoetry {
                Snippet = poetry.Snippet,
                Name = poetry.Name,
                Dynasty = poetry.Dynasty,
                AuthorName = poetry.AuthorName,
                Content = poetry.Content,
                Source = TodayPoetrySources.LOCAL
            };
        }
    }

    public class JinrishiciToken {
        public string Data { get; set; }
    }

    public class JinrishiciOrigin {
        public string Title { get; set; }
        public string Dynasty { get; set; }
        public string Author { get; set; }
        public List<string> Content { get; set; }
        public List<string> Translate { get; set; }
    }

    public class JinrishiciData {
        public string Content { get; set; }
        public JinrishiciOrigin Origin { get; set; }
    }

    public class JinrishiciSentence {
        public JinrishiciData Data { get; set; }
    }
}