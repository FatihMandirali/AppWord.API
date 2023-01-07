using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.BaseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class UserUnknownWordService : Repository<UserUnknownWord>, IUserUnknownWordService
    {
        public UserUnknownWordService(AppWordDbContext context) : base(context)
        {
        }

        public async Task SaveUserUnknownWord(List<int> wordIdList, int userId)
        {
            var userUnknownWord = new List<UserUnknownWord>();
            foreach (var word in wordIdList)
            {
                userUnknownWord.Add(new UserUnknownWord
                {
                    UserId=userId,
                    WordId=word
                });
            }

            await AddRangeAsync(userUnknownWord);
        }
        public async Task<string> RemoveUserUnknownWord(int wordId, int userId)
        {
            var userUnknownWord = await FindAsync(x => x.WordId == wordId && x.UserId == userId && x.IsActive && !x.IsDeleted);
            if (userUnknownWord == null)
                return "generic_notfound_message";
            userUnknownWord.IsDeleted = true;
            userUnknownWord.IsActive = false;
            await UpdateAsync(userUnknownWord, userUnknownWord.Id);
            return null;
        }

        public async Task<PaginatedList<object>> GetUnknownWordList(PaginatedRequest request, int userId)
        {
            var query = FindBy(x => !x.IsDeleted && x.IsActive && x.UserId == userId);
            query = query.Include(x=>x.Word);
            var data = await PaginatedList<object>.CreateAsync(query.Select(x => new
            {
                Id = x.Id,
                WordEn = x.Word.WordEn,
                WordTr = x.Word.WordTr,
            }), request.Page, request.Limit);

            return data;
        }
    }
}
