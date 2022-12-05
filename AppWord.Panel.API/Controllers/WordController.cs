using AppWord.Core.IServices;
using AppWord.Model.Models.Request;
using AppWord.Panel.API.Localize;
using FM.Project.BaseLibrary.BaseResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AppWord.Panel.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly IStringLocalizer<Resource> _localizer;


        public WordController(IWordService wordService, IStringLocalizer<Resource> localizer)
        {
            _wordService = wordService;
            _localizer = localizer;
        }
        /// <summary>
        /// For python service
        /// </summary>
        /// <param name="wordRequest"></param>
        /// <returns></returns>
        [HttpPost("ListCreate")]
        public async Task<FMBaseResponse<object>> CreateListWord([FromBody] List<WordRequest> wordRequest)
        {
            await _wordService.CreateWordList(wordRequest, CurrentUserId);
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }
        /// <summary>
        /// For Python Service
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListAll")]
        public async Task<FMBaseResponse<object>> ListAll()
        {
            var response = await _wordService.ListAll();
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpPost("Create")]
        public async Task<FMBaseResponse<object>> CreateWord([FromBody] WordRequest wordRequest)
        {
            var response = await _wordService.CreateWord(wordRequest, CurrentUserId);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }

        [HttpGet("List")]
        public async Task<FMBaseResponse<object>> List([FromQuery] WordListRequest wordListRequest)
        {
            var response = await _wordService.List(wordListRequest);
            
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpGet("Detail/{id}")]
        public async Task<FMBaseResponse<object>> Detail(int id)
        {
            var response = await _wordService.Detail(id);
            
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpPut("Update/{id}")]
        public async Task<FMBaseResponse<object>> Update([FromBody] WordRequest wordRequest,int id)
        {
            var response = await _wordService.Update(wordRequest,id);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<FMBaseResponse<object>> Delete(int id)
        {
            var response = await _wordService.Delete(id);
            if(response is not null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response);
        }
    }
}
