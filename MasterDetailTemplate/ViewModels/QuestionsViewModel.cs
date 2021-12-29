using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using MasterDetailTemplate.Services.Implementations;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace MasterDetailTemplate.ViewModels
{

    // 调用 QuestionService 提供的方法
    public class QuestionsViewModel: ViewModelBase {

        // 用于判断是否开始加载的标志位
        private bool _canLoadMore;

        //=====================================查询条件==========================================================
        private Expression<Func<Question, bool>> _where;

        public Expression<Func<Question, bool>> Where
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


        // 错题集合
        public InfiniteScrollCollection<Question> QustionCollection { get; set; }
        public InfiniteScrollCollection<Question> QustionCollectionForCategory { get; set; }
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
        // 导航服务
        private IContentNavigationService _contentNavigationService;

        // 提供错题增删改查的基础类
        private IQuestionService _questionService;
        private IQuestionCategoryService _questionCategoryService;
        private IAlertService _alertService;

        public QuestionsViewModel(IQuestionService questionService, IQuestionCategoryService questionCategoryService, IAlertService alertService) {
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 错题相关
            _questionService = questionService;
            _questionCategoryService = questionCategoryService;
            _alertService = alertService;
            //错题类别
            QustionCollectionForCategory = new InfiniteScrollCollection<Question>();
            // 初始化集合
            Where = Expression.Lambda<Func<Question, bool>>(
               // 1. 条件语句
               Expression.Constant(true),
               // 2. 返回的参数?
               Expression.Parameter(typeof(Question), "poetry"));
            QustionCollection = new InfiniteScrollCollection<Question>();
            QustionCollection.OnCanLoadMore = () => _canLoadMore;
            QustionCollection.OnLoadMore = async () => {
                // 开始加载
                IList<Question> list = await _questionService.GetQuestionList(Where,
                    QustionCollection.Count, 20);
                Status = string.Empty;

                if (list.Count < 20)
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (QustionCollection.Count == 0 && list.Count == 0)
                    Status = NoResult;

                // foreach(Question question in list)
                // {
                //     QuestionCategory category = await _questionCategoryService.GetQuestionCategory(question.CategoryId);
                //     question.CategoryName = category.Name;
                // }

                return list;
            };
        }

        // 显示所有错题
        public RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(
                async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction() {
            // 1.判断数据库是否已经被初始化过:如果初始化过, 那么就不需要再继续初始化了，否则会覆盖此前修改的内容
            if(!_questionService.Initialized())
                await _questionService.InitializeAsync();
            // 2.System.InvalidOperationException:“Collection was modified; enumeration operation may not execute.”
            QustionCollection.Clear();
            _canLoadMore = true;
            // 3. 开始从数据库中读取相应的数据
            await QustionCollection.LoadMoreAsync();
            // 4.关闭数据库连接
            _questionService.CloseConnection();

        }



        // 点击错题单元格后自动跳转到错题详情页
        private RelayCommand<Question> _questionTappedCommand;
        public RelayCommand<Question> QuestionTappedCommand =>
            _questionTappedCommand ?? (_questionTappedCommand =
                new RelayCommand<Question>(async question =>
                    await QuestionTappedCommandFunction(question)));
        internal async Task QuestionTappedCommandFunction(Question question) =>
            await _contentNavigationService.NavigateToAsync(
                ContentNavigationServiceConstants.QuestionDetail, question);

        //新增错题命令
        private RelayCommand _toQuestionInsertCommand;

        public RelayCommand ToQuestionInsertCommand => 
            _toQuestionInsertCommand ?? (_toQuestionInsertCommand = 
            new RelayCommand(async ()=> 
                await ToQuestionInsertCommandFunction()));

        public async Task ToQuestionInsertCommandFunction()
        {
            await _contentNavigationService.NavigateToAsync(ContentNavigationServiceConstants.NewQuestionPage);
        }

        // 根据错题类别过滤错题
        private RelayCommand _questionCategoryCommand;
        public RelayCommand QuestionCategoryCommand =>
            _questionCategoryCommand ?? (_questionCategoryCommand = new RelayCommand(
                async () => await QuestionCategoryCommandFuction()));
        
        internal async Task QuestionCategoryCommandFuction() {
            // 1.判断数据库是否已经被初始化过:如果初始化过, 那么就不需要再继续初始化了，否则会覆盖此前修改的内容
            if (!_questionService.Initialized())
                await _questionService.InitializeAsync();
            // 2.System.InvalidOperationException:“Collection was modified; enumeration operation may not execute.”
            QustionCollection.Clear();
            _canLoadMore = true;
            // 3. 开始从数据库中读取相应的数据
            await QustionCollection.LoadMoreAsync();
            // 4.关闭数据库连接
            _questionService.CloseConnection();
            QustionCollectionForCategory.Clear();
            foreach (Question question in QustionCollection) {
                    if (question.CategoryName==QuestionCategory.Name) {
                        QustionCollectionForCategory.Add(question);
                    }
                }
            }

        // 删除错题类别
        private RelayCommand _deleteCategoryCommand;
        public RelayCommand DeleteCategoryCommand =>
            _deleteCategoryCommand ?? (_deleteCategoryCommand = new RelayCommand(
                async () => await DeleteCategoryCommandFuction()));


        internal async Task DeleteCategoryCommandFuction() {
            if (QustionCollectionForCategory.Count!=0) {
                
            } else {
               await _questionCategoryService.DeleteQuestionCategory(QuestionCategory.Id);
               await _contentNavigationService.PopAsync();
            }
        }

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4, string v5)
        {
            throw new NotImplementedException();
        }
    }
}
