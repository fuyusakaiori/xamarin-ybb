using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using MasterDetailTemplate.Services;

namespace MasterDetailTemplate.ViewModels {
    public class ViewModelLocator {

        public QuestionsViewModel QuestionsViewModel =>
            SimpleIoc.Default.GetInstance<QuestionsViewModel>();
        public QuestionDetailViewModel QuestionDetailViewModel =>
            SimpleIoc.Default.GetInstance<QuestionDetailViewModel>();

        public NewQuestionViewModel NewQuestionViewModel =>
            SimpleIoc.Default.GetInstance<NewQuestionViewModel>();
        public QuestionCategoryViewModel QuestionCategoryViewModel =>
            SimpleIoc.Default.GetInstance<QuestionCategoryViewModel>();
        public ViewModelLocator() {
            SimpleIoc.Default.Register<IPreferenceStorage, PreferenceStorage>();
            SimpleIoc.Default.Register<QuestionsViewModel>();
            SimpleIoc.Default.Register<QuestionDetailViewModel>();
            SimpleIoc.Default.Register<IQuestionService, QuestionService>();
            SimpleIoc.Default.Register<IQuestionCategoryService, QuestionCategoryService>();
            SimpleIoc.Default.Register<IAlertService, AlertService>();
            SimpleIoc.Default.Register<NewQuestionViewModel>();
            SimpleIoc.Default.Register<QuestionCategoryViewModel>();
        }
    }
}