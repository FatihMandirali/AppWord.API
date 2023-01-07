using AppWord.Core.IServices;
using AppWord.Model.Models.Options;
using FM.Project.BaseLibrary.BaseResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserPrivateWordCountAttribute:Attribute, IAsyncActionFilter
    {
        

        public UserPrivateWordCountAttribute()
        {
            
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _wordService = context.HttpContext.RequestServices.GetRequiredService<IWordService>();
            var _quizSettings = context.HttpContext.RequestServices.GetRequiredService<QuizSettings>();
            var userId = Convert.ToInt32((context.HttpContext.User?.Identity as ClaimsIdentity)?.FindFirst("Id")?.Value);
            var wordCount = await _wordService.FindBy(x => x.IsActive && !x.IsDeleted && x.UserId == userId && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPrivate).CountAsync();
            if (wordCount < _quizSettings.MinWordCount)
            {
                var responseObj = new FMBaseResponse<object>(FMProcessStatusEnum.BadRequest, new FMFriendlyMessage { Message = $"Quize başlamak için kütüphanenizde en az {_quizSettings.MinWordCount} kelime olması gerekmekte" }, null);


                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            await next();
        }
    }
}
