using AppWord.API.Localize;
using AppWord.Core.Services;
using AppWord.Core.IServices;
using AppWord.Model.Models.Request;
using FM.Project.BaseLibrary.BaseResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AppWord.API.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IStringLocalizer<Resource> localizer, IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<FMBaseResponse<object>> PostLogin([FromBody] LoginRequest postLogin)
        {
            var response = await _authenticationService.Login(postLogin,Data.EntityEnum.RoleEnum.User);
            if (response.Item1 is null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response.Item2] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response.Item1);
        }

        [HttpPost("Register")]
        public async Task<FMBaseResponse<object>> PostRegister([FromBody] RegisterRequest registerRequest)
        {
            var response = await _authenticationService.Register(registerRequest);
            if (response.Item1 is null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response.Item2] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response.Item1);
        }

        [HttpPost("ForgetPassword")]
        public async Task<FMBaseResponse<object>> PostForgetPassword()
        {
            //todo: mail gönderme ile yeni şifre atanacak.
            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, null);
        }
    }
}
