using MasterDetailTemplate.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public class QuestionCategoryService : IQuestionCategoryService
    {
        public IQuestionService _quesitonService;

        public QuestionCategoryService(IQuestionService questionService)
        {
            _quesitonService = questionService;
        }


        public async Task<QuestionCategory> GetQuestionCategory(int id)
        {
            return await _quesitonService.GetConnection().Table<QuestionCategory>().
                Where(category => category.Id == id).FirstOrDefaultAsync();
        }
    }
}
