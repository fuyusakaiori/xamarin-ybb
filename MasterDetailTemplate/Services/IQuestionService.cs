using MasterDetailTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailTemplate.Services
{
    public interface IQuestionService
    {
        // 初始化数据库
        Task InitializeAsync();

        // 添加错题
        Task CreateQuestion(Question question);

        // 删除错题
        Task DeleteQuestion(int id);

        // 更新错题
        Task UpdateQuestion(Question question);

        // 获取所有错题
        Task<IList<Question>> GetQuestionList(
            Expression<Func<Question, bool>> @where, int skip, int take);

        // 按照编号获取错题
        Task GetQuestion(int id);
    }
}
