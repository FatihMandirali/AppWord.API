using AppWord.Core.Helpers.JWT;
using AppWord.Data.EntityEnum;
using AppWord.Model.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IAuthenticationService
    {
        Task<(AccessToken, string)> Login(LoginRequest request,RoleEnum roleEnum);
        Task<(AccessToken, string)> Register(RegisterRequest request);
    }
}
