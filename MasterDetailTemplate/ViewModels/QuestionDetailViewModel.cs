using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using MasterDetailTemplate.Services.Implementations;
using Xamarin.Forms;

namespace MasterDetailTemplate.ViewModels
{
    public class QuestionDetailViewModel :ObservableObject
    {

        private IQuestionService _questionService;

        // 导航服务
        private IContentNavigationService _contentNavigationService;

        private IPhotoPickerService _iphotoPickerService;


        // 新增选取图片的功能
        public QuestionDetailViewModel(IQuestionService questionService,IPhotoPickerService iphotoPickerService) {
            _questionService = questionService;
            _iphotoPickerService = iphotoPickerService;
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
            // 前端接收到的图片
            _image = new Image();
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
            await _questionService.UpdateQuestion(Question);
            _questionService.CloseConnection();
            _contentNavigationService.PopAsync();
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
