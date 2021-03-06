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

namespace MasterDetailTemplate.ViewModels
{
    public class NewQuestionViewModel: ObservableObject
    {
        // 用于判断是否开始加载的标志位
        private bool _canLoadMore;

        //=====================================查询条件==========================================================
        private Expression<Func<QuestionCategory, bool>> _where;

        public Expression<Func<QuestionCategory, bool>> Where
        {
            get => _where;
            // 确定需要更新的属性名称, 然后获取属性的引用, 最后更新值
            set => Set(nameof(Where), ref _where, value);
        }

        //======================================无限滚动读取过程的提示信息=========================================

        private string _status;

        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }

        public const string Loading = "正在载入";

        public const string NoResult = "没有满足条件的结果";

        public const string NoMoreResult = "没有更多结果";

        public InfiniteScrollCollection<string>
            QuestionCategoryCollection
        { get; set; }

        public string SelectedColorName
        {
            get => _selectedColorName;
            set => Set(nameof(SelectedColorName), ref _selectedColorName, value);
        }
        private string _selectedColorName;
        private IQuestionService _questionService;
        private IQuestionCategoryService _questionCategoryService;
        // 导航服务
        private IContentNavigationService _contentNavigationService;

        public NewQuestionViewModel(IQuestionService questionService,IQuestionCategoryService questionCategoryService) {
            _questionService = questionService;
            _questionCategoryService = questionCategoryService;
            _question =new Question();
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 初始化集合
            Where = Expression.Lambda<Func<QuestionCategory, bool>>(
                // 1. 条件语句
                Expression.Constant(true),
                // 2. 返回的参数?
                Expression.Parameter(typeof(QuestionCategory), "poetry"));
            QuestionCategoryCollection = new InfiniteScrollCollection<string>();
            QuestionCategoryCollection.OnCanLoadMore = () => _canLoadMore;
            QuestionCategoryCollection.OnLoadMore = async () => {
                // 开始加载
                IList<QuestionCategory> list1 =
                    await _questionCategoryService.GetQuestionCategoryList(
                        Where, QuestionCategoryCollection.Count, 20);
                Status = string.Empty;
                if (list1.Count < 20)
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (QuestionCategoryCollection.Count == 0 && list1.Count == 0)
                    Status = NoResult;
                IList<string> list2 = new List<string>();
                foreach (QuestionCategory questionCategory in list1)
                {
                    list2.Add(questionCategory.Name);
                }
                return list2;
            };


        }
        /// <summary>
        /// 新增的错题。
        /// </summary>
        public Question Question
        {
            get => _question;
            set => Set(nameof(Question), ref _question, value);
        }

        /// <summary>
        /// 新增的错题。
        /// </summary>
        public Question _question;

        /******** 绑定命令 ********/

        /// <summary>
        /// 页面显示命令。
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));



        /// <summary>
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _pageAppearingCommand;

        internal async Task PageAppearingCommandFunction() {
            // 1.判断数据库是否已经被初始化过:如果初始化过, 那么就不需要再继续初始化了，否则会覆盖此前修改的内容
            if (!_questionService.Initialized())
                await _questionService.InitializeAsync();
            // 2.System.InvalidOperationException:“Collection was modified; enumeration operation may not execute.”
            QuestionCategoryCollection.Clear();
            _canLoadMore = true;
            // 3. 开始从数据库中读取相应的数据
            await QuestionCategoryCollection.LoadMoreAsync();
            // 4.关闭数据库连接
            _questionService.CloseConnection();
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

        internal async Task AddCommandFunction() {
            Question.CategoryName = SelectedColorName;
            await _questionService.CreateQuestion(Question);
            _questionService.CloseConnection();
            await _contentNavigationService.PopAsync();
        }
    }
}
