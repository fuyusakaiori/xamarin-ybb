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
    public class QuestionDetailViewModel :ObservableObject
    {

        private IQuestionService _questionService;

        // 导航服务
        private IContentNavigationService _contentNavigationService;

        public QuestionDetailViewModel(IQuestionService questionService) {
            _questionService = questionService;
            // 导航相关
            _contentNavigationService = new ContentNavigationService(
                new CachedContentPageActivationService());
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
            System.Diagnostics.Debug.WriteLine(Question.Id + "\t" + Question.Name + "\t" + Question.Content);
            await _questionService.UpdateQuestion(Question);
            _questionService.CloseConnection();
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
            System.Diagnostics.Debug.WriteLine(Question.Id + "\t" + Question.Name + "\t" + Question.Content);
            await _questionService.DeleteQuestion(Question.Id);
            _questionService.CloseConnection();
            await _contentNavigationService.PopAsync();

        }
    }
}
