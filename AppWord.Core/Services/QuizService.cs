using AppWord.Core.IServices;
using AppWord.Model.Models.Options;
using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class QuizService : IQuizService
    {
        private readonly IWordService _wordService;
        private readonly IUserUnknownWordService _userUnknownWordService;
        private readonly IUserService _userService;
        private readonly QuizSettings _quizSettings;

        public QuizService(IWordService wordService, QuizSettings quizSettings, IUserService userService, IUserUnknownWordService userUnknownWordService)
        {
            _wordService = wordService;
            _quizSettings = quizSettings;
            _userService = userService;
            _userUnknownWordService = userUnknownWordService;
        }

        public async Task<(string, List<WordResponse>)> GetUserPrivateQuiz(int userId)
        {
            var wordCount = await _wordService.FindBy(x => x.IsActive && !x.IsDeleted && x.UserId == userId && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPrivate).CountAsync();
            if (wordCount < _quizSettings.MinWordCount)
                return ("quizMinCount", null);


            Random random = new Random();
            var randomBetween = wordCount - (_quizSettings.QuizQuestionCount);
            var rndm = random.Next(randomBetween);
            var quizWord = await _wordService.FindBy(x => x.IsActive && !x.IsDeleted && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPrivate && x.UserId == userId).AsNoTracking().Skip(rndm).Take(_quizSettings.QuizQuestionCount).Select(x => new WordResponse
            {
                Id = x.Id,
                WordEn = x.WordEn,
                WordTr = x.WordTr,
                CreateDate = x.CreateDate,
                IsActive = x.IsActive,
            }).ToListAsync();
            return (null, quizWord);
        }

        public async Task<string> ApplyPrivateQuiz(ApplyPrivateQuizRequest applyPrivateQuizRequest, int userId)
        {
            var user = await _userService.FindAsync(x => x.Id == userId);
            if (user == null)
                return "generic_notfound_message";
            user.TotalTrueCount += applyPrivateQuizRequest.TrueCount;
            user.TotalFalseCount += applyPrivateQuizRequest.FalseCount;
            await _userService.UpdateAsync(user, user.Id);

            if (!applyPrivateQuizRequest.FalseWordIdList.Any())
                return null;

            await _userUnknownWordService.SaveUserUnknownWord(applyPrivateQuizRequest.FalseWordIdList, userId);

            return null;
        }
    }
}
