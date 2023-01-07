using AppWord.API.Localize;
using AppWord.Core.IServices;
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
    public class AnnouncementController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, IAnnouncementService announcementService)
        {
            _localizer = localizer;
            _logger = logger;
            _announcementService = announcementService;
        }

        [HttpGet("GetAnnouncementGeneralList")]
        public async Task<FMBaseResponse<List<AnnouncementResponse>>> AnnouncementGeneralList()
        {
            //todo: repsonse cache yapılacak
            var response = await _announcementService.ActiveAnnouncementGeneralList();
            return new FMBaseResponse<List<AnnouncementResponse>>(FMProcessStatusEnum.Success, null, response);
        }

        [HttpGet("GetAnnouncementListByUser")]
        public async Task<FMBaseResponse<List<AnnouncementResponse>>> AnnouncementListByUser()
        {
            var response = await _announcementService.ActiveAnnouncementListByUser(CurrentUserId);
            return new FMBaseResponse<List<AnnouncementResponse>>(FMProcessStatusEnum.Success, null, response);
        }

    }
}
