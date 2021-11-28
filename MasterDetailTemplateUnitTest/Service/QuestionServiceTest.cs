using System.Threading.Tasks;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Services;
using NUnit.Framework;

namespace MasterDetailTemplateUnitTest.Service
{
    public class QuestionServiceTest
    {
        [Test]
        public async Task UpdateQuestion() {
            QuestionService service = new QuestionService();
            await service.InitializeAsync();
            Question question = await service.GetQuestion(1);
            question.Name = "超市你";
            await service.UpdateQuestion(question);
        }
    }
}