using MasterDetailTemplate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public interface IQuestionCategoryService
    {
        Task<QuestionCategory> GetQuestionCategory(int id);
    }
}
