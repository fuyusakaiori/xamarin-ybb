using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using MasterDetailTemplate.Services.Implementations;

namespace MasterDetailTemplate.ViewModels
{
    public class NewQuestionViewModel: ObservableObject
    {
        private IQuestionService _questionService;
        // 导航服务
        private IContentNavigationService _contentNavigationService;

        public NewQuestionViewModel(IQuestionService questionService) {
            _questionService = questionService;
            _question=new Question();
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
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
            System.Diagnostics.Debug.WriteLine(Question.Id + "\t" + Question.Name + "\t" + Question.Content);
            await _questionService.CreateQuestion(Question);
            _questionService.CloseConnection();
            await _contentNavigationService.PopAsync();
        }
    }
}
