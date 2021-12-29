using MasterDetailTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public interface IQuestionCategoryService
    {
        //根据id查找错题类型
        Task<QuestionCategory> GetQuestionCategory(int id);

        //返回错题类型
        Task<IList<QuestionCategory>> GetQuestionCategoryList(
            Expression<Func<QuestionCategory, bool>> @where, int skip, int take);

        Task<IList<QuestionCategory>> GetAllQuestionCategoryList();
        ////新增错题类型
        Task CreateQuestionCategory(QuestionCategory questionCategory);
        //删除
        Task DeleteQuestionCategory(int id);
        //修改
        Task UpdateQuestionCategory(QuestionCategory questionCategory);
    }
}
