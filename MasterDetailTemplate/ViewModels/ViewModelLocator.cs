using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using MasterDetailTemplate.Services;

namespace MasterDetailTemplate.ViewModels {
    public class ViewModelLocator {
        public ResultPageViewModel ResultPageViewModel =>
            SimpleIoc.Default.GetInstance<ResultPageViewModel>();

        public TodayPageViewModel TodayPageViewModel =>
            SimpleIoc.Default.GetInstance<TodayPageViewModel>();

        public QuestionsViewModel QuestionsViewModel =>
            SimpleIoc.Default.GetInstance<QuestionsViewModel>();
        public QuestionDetailViewModel QuestionDetailViewModel =>
            SimpleIoc.Default.GetInstance<QuestionDetailViewModel>();

        public ViewModelLocator() {
            SimpleIoc.Default.Register<ResultPageViewModel>();
            SimpleIoc.Default.Register<IPoetryStorage, PoetryStorage>();
            SimpleIoc.Default.Register<IPreferenceStorage, PreferenceStorage>();
            SimpleIoc.Default.Register<ITodayImageService, BingImageService>();
            SimpleIoc.Default.Register<ITodayImageStorage, TodayImageStorage>();
            SimpleIoc.Default.Register<ITodayPoetryService, JinrishiciService>();
            SimpleIoc.Default.Register<TodayPageViewModel>();
            SimpleIoc.Default.Register<IAlertService,AlertService>();
            SimpleIoc.Default.Register<QuestionsViewModel>();
            SimpleIoc.Default.Register<QuestionDetailViewModel>();
            SimpleIoc.Default.Register<IQuestionService, QuestionService>();
        }
    }
}