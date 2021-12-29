using MasterDetailTemplate.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
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
        //获取全部错题类型
        public async Task<IList<QuestionCategory>> GetQuestionCategoryList(
            Expression<Func<QuestionCategory, bool>> @where, int skip, int take)
        {
            List<QuestionCategory> list = await _quesitonService.GetConnection().Table<QuestionCategory>().
                Where(@where).Skip(skip).Take(take).ToListAsync();
            return list;
        }
        //新增错题类型
        public async Task CreateQuestionCategory(QuestionCategory questionCategory)
        {
            await _quesitonService.GetConnection().InsertAsync(questionCategory);
        }

        //删除
        public async Task DeleteQuestionCategory(int id)
        {
            await _quesitonService.GetConnection().Table<QuestionCategory>()
                .DeleteAsync(questionCategory => questionCategory.Id == id);
        }

        //修改
        public async Task UpdateQuestionCategory(QuestionCategory questionCategory)
        {
            await _quesitonService.GetConnection().UpdateAsync(questionCategory);
        }

        public async Task<IList<QuestionCategory>> GetAllQuestionCategoryList()
        {
            List<QuestionCategory> list = await _quesitonService.GetConnection().Table<QuestionCategory>().ToListAsync();
            return list;
        }
    }
}
