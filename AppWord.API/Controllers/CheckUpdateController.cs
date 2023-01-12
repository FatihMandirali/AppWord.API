using AppWord.API.Localize;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CheckUpdateController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly ICheckUpdateService _checkUpdateService;
        public CheckUpdateController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, ICheckUpdateService checkUpdateService)
        {
            _localizer = localizer;
            _logger = logger;
            _checkUpdateService = checkUpdateService;
        }

        [HttpPost("CheckUpdate")]
        public async Task<FMBaseResponse<object>> GetOnboarding([FromBody]CheckUpdateRequest request)
        {
            var response = await _checkUpdateService.CheckUpdate(request);
            if(response == "minor_update")
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);
            if(response=="major_update")
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response] }, null);
            
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }

    }
}
