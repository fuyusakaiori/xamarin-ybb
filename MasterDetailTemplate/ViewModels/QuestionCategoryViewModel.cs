using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using MasterDetailTemplate.Services.Implementations;
using Xamarin.Forms.Extended;

namespace MasterDetailTemplate.ViewModels {
    public class QuestionCategoryViewModel : ViewModelBase {
        // 用于判断是否开始加载的标志位
        private bool _canLoadMore;

        //=====================================查询条件==========================================================
        private Expression<Func<QuestionCategory, bool>> _where;

        public Expression<Func<QuestionCategory, bool>> Where {
            get => _where;
            // 确定需要更新的属性名称, 然后获取属性的引用, 最后更新值
            set => Set(nameof(Where), ref _where, value);
        }

        //======================================无限滚动读取过程的提示信息=========================================

        private string _status;

        public string Status {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        public const string Loading = "正在载入";

        public const string NoResult = "没有满足条件的结果";

        public const string NoMoreResult = "没有更多结果";

        /// <summary>
        /// 错题类别。
        /// </summary>
        public QuestionCategory QuestionCategory
        {
            get => _questionCategory;
            set => Set(nameof(QuestionCategory), ref _questionCategory, value);
        }

        /// <summary>
        /// 错题类别。
        /// </summary>
        public QuestionCategory _questionCategory;

        // 错题集合
        public InfiniteScrollCollection<QuestionCategory>
            QuestionCategoryCollection { get; set; }

        // 导航服务
        private IContentNavigationService _contentNavigationService;

        // 提供错题增删改查的基础类
        private IQuestionService _questionService;
        private IQuestionCategoryService _questionCategoryService;


        public QuestionCategoryViewModel(
            IQuestionService questionService, IQuestionCategoryService questionCategoryService) {
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 错题相关
            _questionService = questionService;
            _questionCategoryService = questionCategoryService;
            _questionCategory = new QuestionCategory();
            // 初始化集合
            Where = Expression.Lambda<Func<QuestionCategory, bool>>(
                // 1. 条件语句
                Expression.Constant(true),
                // 2. 返回的参数?
                Expression.Parameter(typeof(QuestionCategory), "poetry"));
            QuestionCategoryCollection = new InfiniteScrollCollection<QuestionCategory>();
            QuestionCategoryCollection.OnCanLoadMore = () => _canLoadMore;
            QuestionCategoryCollection.OnLoadMore = async () => {
                // 开始加载
                IList<QuestionCategory> list =
                    await _questionCategoryService.GetQuestionCategoryList(
                        Where,QuestionCategoryCollection.Count, 20);
                Status = string.Empty;
                if (list.Count < 20) {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (QuestionCategoryCollection.Count == 0 && list.Count == 0)
                    Status = NoResult;

                return list;
            };
        }
        // 显示所有错题
        public RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            // 1.判断数据库是否已经被初始化过:如果初始化过, 那么就不需要再继续初始化了，否则会覆盖此前修改的内容
            if (!_questionService.Initialized())
                await _questionService.InitializeAsync();
            // 2.清理集合中的内容并继续读取
            QuestionCategoryCollection.Clear();
            _canLoadMore = true;
            // 3. 开始从数据库中读取相应的数据
            await QuestionCategoryCollection.LoadMoreAsync();
            // 4.关闭数据库连接
            _questionService.CloseConnection();
        }
        private RelayCommand<QuestionCategory> _questionTappedCommand;
        public RelayCommand<QuestionCategory> QuestionTappedCommand =>
            _questionTappedCommand ?? (_questionTappedCommand =
                new RelayCommand<QuestionCategory>(async questionCategory =>
                    await QuestionTappedCommandFunction(questionCategory)));
        internal async Task QuestionTappedCommandFunction(QuestionCategory questionCategory) =>
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationServiceConstants.QuestionsPageForCategory, questionCategory);

        //新增错题命令
        private RelayCommand _toQuestionInsertCommand;

        public RelayCommand ToQuestionInsertCommand =>
            _toQuestionInsertCommand ?? (_toQuestionInsertCommand =
                new RelayCommand(async () =>
                    await ToQuestionInsertCommandFunction()));

        public async Task ToQuestionInsertCommandFunction()
        {
            await _contentNavigationService.NavigateToAsync(ContentNavigationServiceConstants.NewCategoryPage);
        }


        /// <summary>
        /// 新增题目命令。
        /// </summary>
        public RelayCommand AddCommand =>
            _addCommand ?? (_addCommand = new RelayCommand(
                async () => await AddCommandFunction()));



        /// <summary>
        /// 新增题目命令。
        /// </summary>
        private RelayCommand _addCommand;

        internal async Task AddCommandFunction()
        {
            await _questionCategoryService.CreateQuestionCategory(QuestionCategory);
            _questionService.CloseConnection();
            await _contentNavigationService.PopAsync();
        }
    }

}
