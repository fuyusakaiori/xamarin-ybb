﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MasterDetailTemplate.Models;

namespace MasterDetailTemplate.ViewModels
{
    public class QuestionDetailViewModel :ObservableObject{
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
        /// 页面显示命令。
        /// </summary>
        private RelayCommand _saveCommand;

        internal async Task SaveCommandFunction() {
            System.Diagnostics.Debug.WriteLine("question save"+Question.Name+"  "+Question.Content);
        }
    }
}