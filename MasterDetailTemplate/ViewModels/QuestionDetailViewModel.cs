using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
    public class QuestionDetailViewModel :ObservableObject
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

        private IQuestionService _questionService;

        private IQuestionCategoryService _questionCategoryService;

        // 导航服务
        private IContentNavigationService _contentNavigationService;

        private IPhotoPickerService _iphotoPickerService;

        public IList<string> CategoryList { get;set; }

        // 错题集合
        public InfiniteScrollCollection<string>
            QuestionCategoryCollection
        { get; set; }

        public string SelectedColorName
        {
            get => _selectedColorName;
            set => Set(nameof(SelectedColorName), ref _selectedColorName, value);
        }
        private string  _selectedColorName;

        // 新增选取图片的功能
        public QuestionDetailViewModel(IQuestionService questionService,IPhotoPickerService iphotoPickerService,IQuestionCategoryService questionCategoryService, IAlertService alertService) {
            _questionService = questionService;
            _iphotoPickerService = iphotoPickerService;
            _questionCategoryService = questionCategoryService;
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 前端接收到的图片
            _image = new Image();
            CategoryList = new List<string>();

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
                foreach (QuestionCategory questionCategory in list1) {
                    list2.Add(questionCategory.Name);
                }
                return list2;
            };


        }

        /// <summary>
        /// 错题详情。
        /// </summary>
        public Question Question
        {
            get => _question;
            set => Set(nameof(Question), ref _question, value);
        }

        /// <summary>
        /// 错题详情。
        /// </summary>
        public Question _question;



        public Image Image
        {
            get => _image;
            set => Set(nameof(Image), ref _image, value);
        }

        public Image _image;

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

        internal async Task PageAppearingCommandFunction()
        {
            //如果path不为空显示
            if (Question.Path != null)
            {
                Image.Source = Question.Path;
            } else {
                //因为Image为viewmodel层绑定的变量，使用过后要清空
                Image.Source = null;
            }
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

        /// <summary>
        /// 保存命令。
        /// </summary>
        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(
                async () => await SaveCommandFunction()));



        /// <summary>
        ///保存命令。
        /// </summary>
        private RelayCommand _saveCommand;

        internal async Task SaveCommandFunction() {
            Question.CategoryName = SelectedColorName;
            await _questionService.UpdateQuestion(Question);
            _questionService.CloseConnection();
            _contentNavigationService.PopAsync();
            // System.Diagnostics.Debug.WriteLine(SelectedColorName);
        }



        /// <summary>
        /// 删除命令。
        /// </summary>
        public RelayCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new RelayCommand(
                async () => await DeleteCommandFunction()));



        /// <summary>
        /// 删除命令。
        /// </summary>
        private RelayCommand _deleteCommand;

        internal async Task DeleteCommandFunction()
        {
            await _questionService.DeleteQuestion(Question.Id);
            _questionService.CloseConnection();
            await _contentNavigationService.PopAsync();

        }


        /// <summary>
        /// 增加图片命令。
        /// </summary>
        public RelayCommand AddImageCommand =>
            _addImageCommand ?? (_addImageCommand = new RelayCommand(
                async () => await AddImageCommandFunction()));



        /// <summary>
        /// 增加图片命令。
        /// </summary>
        private RelayCommand _addImageCommand;

        //图片路径
        private static string ImagePath;
        private static string ImagePath2;
        internal async Task AddImageCommandFunction() {
            // Image.Source = ImageSource.FromStream(() => stream);
            //从系统中获取图片的流
            Stream stream = await _iphotoPickerService.GetImageStreamAsync();
            if (stream!=null) {
                //因为展示image需要占用图片资源，所以如果需要更换图片，是不能直接覆盖原有图片文件的（会造成线程访问资源的冲突），每次添加图片都会存入一个新的文件，文件命名为8位随机字符
                ImagePath2 = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), RandomString(8)+".png");

                using (var imageFileStream =
                        new FileStream(ImagePath2, FileMode.Create))
                    {
                        await stream.CopyToAsync(imageFileStream); 
                           //更改图片资源
                            Image.Source = ImagePath2;
                            ImagePath = Question.Path;
                            Question.Path = ImagePath2;

                    }
                }

            }
        private static Random random = new Random();
        //生成规定长度的字符串
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
