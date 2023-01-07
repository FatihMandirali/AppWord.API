using AppWord.API.Localize;
using AppWord.Core.Attributes;
using AppWord.Core.IServices;
using AppWord.Data.EntityEnum;
using AppWord.Model.Models.BaseModel;
using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using FM.Project.BaseLibrary.BaseResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AppWord.API.Controllers
{
    [Authorize]
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class QuizController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly IQuizService _quizService;
        public QuizController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, IQuizService quizService)
        {
            _localizer = localizer;
            _logger = logger;
            _quizService = quizService;
        }

        //[UserPrivateWordCount]
        [HttpGet("GetPrivateQuiz")]
        public async Task<FMBaseResponse<List<WordResponse>>> GetPrivateQuiz()
        {
            var response = await _quizService.GetUserPrivateQuiz(CurrentUserId);
            if(response.Item1 is not null)
                return new FMBaseResponse<List<WordResponse>>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response.Item1] }, null);

            return new FMBaseResponse<List<WordResponse>>(FMProcessStatusEnum.Success, null, response.Item2);
        }

        [HttpPost("ApplyPrivateQuiz")]
        public async Task<FMBaseResponse<object>> ApplyPrivateQuiz([FromBody] ApplyPrivateQuizRequest applyPrivateQuizRequest)
        {
            // total puan hesaplama eksik servis tarafında..
            var response = await _quizService.ApplyPrivateQuiz(applyPrivateQuizRequest, CurrentUserId);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }

    }
}
