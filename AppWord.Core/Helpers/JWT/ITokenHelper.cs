using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Helpers.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(RoleEnum rolesEnum, int id, string username, string email);
    }
}
