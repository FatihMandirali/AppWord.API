using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IQuizService
    {
        Task<(string, List<WordResponse>)> GetUserPrivateQuiz(int userId);
        Task<string> ApplyPrivateQuiz(ApplyPrivateQuizRequest applyPrivateQuizRequest, int userId);
    }
}
