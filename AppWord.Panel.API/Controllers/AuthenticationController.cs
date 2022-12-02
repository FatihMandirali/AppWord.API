using AppWord.Core.IServices;
using AppWord.Model.Models.Request;
using AppWord.Panel.API.Localize;
using FM.Project.BaseLibrary.BaseResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AppWord.Panel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<Resource> _localizer;


        public AuthenticationController(IAuthenticationService authenticationService, IStringLocalizer<Resource> localizer)
        {
            _authenticationService = authenticationService;
            _localizer = localizer;
        }

        [HttpPost("Login")]
        public async Task<FMBaseResponse<object>> PostLogin([FromBody] LoginRequest postLogin)
        {
            var response = await _authenticationService.Login(postLogin,Data.EntityEnum.RoleEnum.Admin);
            if (response.Item1 is null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response.Item2] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response.Item1);
        }
    }
}
