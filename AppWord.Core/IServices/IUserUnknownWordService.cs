using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IUserUnknownWordService:IRepository<UserUnknownWord>
    {
        Task SaveUserUnknownWord(List<int> wordIdList, int userId);
        Task<string> RemoveUserUnknownWord(int wordId, int userId);
        Task<PaginatedList<object>> GetUnknownWordList(PaginatedRequest request, int userId);
    }
}
