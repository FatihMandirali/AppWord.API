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
    public class OnboardingController : BaseController
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<UserController> _logger;
        private readonly IOnboardingService _onboardingService;
        public OnboardingController(IStringLocalizer<Resource> localizer, ILogger<UserController> logger, IOnboardingService onboardingService)
        {
            _localizer = localizer;
            _logger = logger;
            _onboardingService = onboardingService;
        }

        [HttpGet("GetOnboarding")]
        public async Task<FMBaseResponse<List<OnboardingResponse>>> GetOnboarding([FromQuery]OnboardingEnum onboardingEnum)
        {
            var response = await _onboardingService.OnboardingList(onboardingEnum);
            return new FMBaseResponse<List<OnboardingResponse>>(FMProcessStatusEnum.Success, null, response);
        }

    }
}
