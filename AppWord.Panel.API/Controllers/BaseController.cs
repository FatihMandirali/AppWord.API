using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppWord.Panel.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public int CurrentUserId => Convert.ToInt32((User?.Identity as ClaimsIdentity)?.FindFirst("Id")?.Value);
    }
}
