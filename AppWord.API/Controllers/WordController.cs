using AppWord.API.Localize;
using AppWord.Core.IServices;
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
    public class WordController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly IWordService _wordService;
        private readonly IUserUnknownWordService _userUnknownWordService;
        public WordController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, IWordService wordService, IUserUnknownWordService userUnknownWordService)
        {
            _localizer = localizer;
            _logger = logger;
            _wordService = wordService;
            _userUnknownWordService = userUnknownWordService;
        }

        [HttpGet("GetWordOfDay")]
        public async Task<FMBaseResponse<List<WordResponse>>> GetWordOfDay()
        {
            var response = await _wordService.WordOfDay();
            return new FMBaseResponse<List<WordResponse>>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpPost("PostSuggestWord")]
        public async Task<FMBaseResponse<object>> PostSuggestWord([FromBody] SuggestWordRequest suggestWordRequest)
        {
            var response = await _wordService.SuggestWord(suggestWordRequest,CurrentUserId);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.BadRequest, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpPost("PostPrivateWord")]
        public async Task<FMBaseResponse<object>> PostPrivateWord([FromBody] SuggestWordRequest suggestWordRequest)
        {
            var response = await _wordService.PrivateWord(suggestWordRequest,CurrentUserId);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.BadRequest, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpGet("GetPrivateWord")]
        public async Task<FMBaseResponse<PaginatedList<WordResponse>>> GetPrivateWord([FromQuery] WordListRequest wordListRequest)
        {
            var response = await _wordService.PrivateWordList(wordListRequest,CurrentUserId);
            return new FMBaseResponse<PaginatedList<WordResponse>>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpGet("GetUnknownWordList")]
        public async Task<FMBaseResponse<object>> GetUnknownWordList([FromQuery]PaginatedRequest paginatedRequest)
        {
            var response = await _userUnknownWordService.GetUnknownWordList(paginatedRequest, CurrentUserId);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpPost("PostUnknownWordAdd")]
        public async Task<FMBaseResponse<object>> PostUnknownWordAdd([FromBody] UnknownWordAddRequest unknownWordAddRequest)
        {
            await _userUnknownWordService.SaveUserUnknownWord(unknownWordAddRequest.WordIdList, CurrentUserId);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }

        [HttpDelete("PostUnknownWordRemove")]
        public async Task<FMBaseResponse<object>> PostUnknownWordRemove(int wordId)
        {
            var response = await _userUnknownWordService.RemoveUserUnknownWord(wordId, CurrentUserId);
            if (response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.BadRequest, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

    }
}
