using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.BaseModel;
using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class WordService : Repository<Word>, IWordService
    {
        public WordService(AppWordDbContext context) : base(context)
        {
        }

        public async Task CreateWordList(List<WordRequest> wordRequests, int adminId)
        {
            foreach (WordRequest wordRequest in wordRequests)
            {
                var word = new Word();
                word.WordTr = wordRequest.WordTr.ToLower();
                word.WordEn = wordRequest.WordEn.ToLower();
                word.UserId = adminId;
                word.WordStatusEnum = Data.EntityEnum.WordStatusEnum.IsPublic;
                await AddAsync(word);
            }

        }
        public async Task<List<string>> ListAll()
        {
            var words = await FindBy(x=>x.IsActive).Select(x => x.WordEn).ToListAsync();
            return words;
        }

        public async Task<string> CreateWord(WordRequest wordRequest, int adminId)
        {
            var existWord = await FindAsync(x => x.WordTr == wordRequest.WordTr);
            if (existWord is not null)
                return "existErrorMessage";

            var word = new Word();
            word.WordTr = wordRequest.WordTr;
            word.WordEn = wordRequest.WordEn;
            word.UserId = adminId;
            word.IsActive = wordRequest.IsActive;
            word.WordStatusEnum = Data.EntityEnum.WordStatusEnum.IsPublic;
            await AddAsync(word);
            return null;
        }

        public async Task<PaginatedList<WordResponse>> List(WordListRequest wordListRequest)
        {
            var query = FindBy(x => !x.IsDeleted && x.IsDeleted == false && x.IsActive == wordListRequest.IsActive);
            query = query.Include(x => x.User);
            var data = await PaginatedList<WordResponse>.CreateAsync(query.Select(x => new WordResponse
            {
                Id = x.Id,
                CreateDate = x.CreateDate,
                IsActive = x.IsActive,
                WordEn = x.WordEn,
                WordTr = x.WordTr,
                UserName = x.User.UserName
            }), wordListRequest.Page, wordListRequest.Limit);

            return data;
        }

        public async Task<WordResponse> Detail(int id)
        {
            var query = FindBy(x => x.Id == id);
            query = query.Include(x => x.User);
            var response = await query.Select(x => new WordResponse
            {
                CreateDate = x.CreateDate,
                Id = x.Id,
                IsActive = x.IsActive,
                UserName = x.User.UserName,
                WordEn = x.WordEn,
                WordTr = x.WordTr
            }).FirstOrDefaultAsync();
            return response;
        }

        public async Task<string> Update(WordRequest wordRequest,int id)
        {
            var word = await FindAsync(x => x.Id == id);
            if (word == null)
                return "generic_notfound_message";
            word.WordEn = wordRequest.WordEn;
            word.WordTr = wordRequest.WordTr;
            word.IsActive = wordRequest.IsActive;
            await UpdateAsync(word, id);
            return null;
        }

        public async Task<string> Delete(int id)
        {
            var word = await FindAsync(x => x.Id == id);
            if (word == null)
                return "generic_notfound_message";
            word.IsActive = false;
            word.IsDeleted = true;
            await UpdateAsync(word, id);
            return null;
        }
    }
}
