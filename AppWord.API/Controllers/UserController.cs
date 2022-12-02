using AppWord.API.Localize;
using AppWord.Core.IServices;
using AppWord.Model.Models.Request;
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
    public class UserController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, IUserService userService)
        {
            _localizer = localizer;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("Update")]
        public async Task<FMBaseResponse<object>> PostForgetPassword([FromBody] UserUpdateRequest userUpdateRequest)
        {
            var userId = CurrentUserId;
            var response = await _userService.UpdateUser(userUpdateRequest, userId);
            if(response.Item1 is null)
                return new FMBaseResponse<object>(FMProcessStatusEnum.NotFound, new FMFriendlyMessage { Message = _localizer[response.Item2] }, null);

            return new FMBaseResponse<object>(FMProcessStatusEnum.Success, null, response.Item1);
        }

    }
}
