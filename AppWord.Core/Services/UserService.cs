using AppWord.Core.Helpers.JWT;
using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.EntityEnum;
using AppWord.Data.Repository;
using AppWord.Model.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace AppWord.Core.Services
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly ITokenHelper _tokenHelper;

        public UserService(AppWordDbContext context, ITokenHelper tokenHelper) : base(context)
        {
            _tokenHelper = tokenHelper;
        }

        public async Task<(AccessToken, string)> UpdateUser(UserUpdateRequest userUpdateRequest, int id)
        {
            var existControl = await FindAsync(x => x.Role == RoleEnum.User && x.Id != id && (x.Email == userUpdateRequest.Email || x.UserName == userUpdateRequest.UserName));
            if (existControl is not null)
                return (null,"registerExistErrorMessage");

            var user = await FindAsync(x => x.Id == id);
            if (user is null)
                return (null, "registerExistErrorMessage");

            user.Email = userUpdateRequest.Email;
            user.UserName = userUpdateRequest.UserName;
            user.Password = BC.HashPassword(userUpdateRequest.Password);
            await UpdateAsync(user,id);

            var token = _tokenHelper.CreateToken(RoleEnum.User, user.Id);

            return (token, null);
        }
    }
}
