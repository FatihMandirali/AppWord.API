using AppWord.Core.Helpers.JWT;
using AppWord.Core.IServices;
using AppWord.Data.Entity;
using AppWord.Data.EntityEnum;
using AppWord.Model.Models.Request;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace AppWord.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserService _userService;

        public AuthenticationService(ITokenHelper tokenHelper, IUserService userService)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
        }

        public async Task<(AccessToken, string)> Login(LoginRequest request, RoleEnum roleEnum=RoleEnum.User)
        {
            var user = await _userService.FindAsync(x => x.UserName == request.UserName && x.Role == roleEnum);

            if (user is null)
                return (null, "loginInfoErrorMessage");

            var verify = BC.Verify(request.Password, user.Password);
            if (!verify)
                return (null, "loginInfoErrorMessage");

            var token = _tokenHelper.CreateToken(RoleEnum.User, user.Id);
            return (token, null);
        }

        public async Task<(AccessToken, string)> Register(RegisterRequest request)
        {
            var user = await _userService.FindAsync(x => x.Role == RoleEnum.User && (x.UserName == request.UserName || x.Email == request.Email));

            if (user is not null)
                return (null, "registerExistErrorMessage");

            user = new User();
            user.Email = request.Email;
            user.Password = BC.HashPassword(request.Password);
            user.UserName = request.UserName;
            user.Role = RoleEnum.User;

            await _userService.AddAsync(user);

            var token = _tokenHelper.CreateToken(RoleEnum.User, user.Id);
            return (token, null);
        }
    }
}
