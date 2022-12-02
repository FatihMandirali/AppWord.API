using AppWord.Core.Helpers.JWT;
using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IUserService : IRepository<User>
    {
        Task<(AccessToken,string)> UpdateUser(UserUpdateRequest userUpdateRequest, int id);
    }
}
