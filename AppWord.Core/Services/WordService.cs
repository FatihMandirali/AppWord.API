using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.BaseModel;
using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public WordService(AppWordDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
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
            var query = FindBy(x => !x.IsDeleted && x.IsActive == wordListRequest.IsActive);
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
        public async Task<List<WordResponse>> WordOfDay()
        {
            var today = DateTime.Today;
            var response = await FindBy(x => x.IsActive && x.WordOfDayDate == today).Include(x=>x.User).ToListAsync();
            var mapResponse = _mapper.Map<List<WordResponse>>(response);

            return mapResponse;
        }

        public async Task<string> SuggestWord(SuggestWordRequest suggestWordRequest, int id)
        {
            var wordControl = await FindAsync(x => x.WordEn.ToLower() == suggestWordRequest.WordEn && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPublic);
            if (wordControl is not null)
                return "suggestWordAlreadyExist";
            var word = new Word();
            word.WordTr = suggestWordRequest.WordTr;
            word.WordEn = suggestWordRequest.WordEn;
            word.UserId = id;
            word.WordStatusEnum = Data.EntityEnum.WordStatusEnum.IsWaiting;
            await AddAsync(word);
            return null;
        }

        public async Task<string> PrivateWord(SuggestWordRequest suggestWordRequest, int id)
        {
            var wordControl = await FindAsync(x => x.WordEn.ToLower() == suggestWordRequest.WordEn && x.UserId == id && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPrivate);
            if (wordControl is not null)
                return "wordAlreadyExist";
            var word = new Word();
            word.WordTr = suggestWordRequest.WordTr;
            word.WordEn = suggestWordRequest.WordEn;
            word.UserId = id;
            word.WordStatusEnum = Data.EntityEnum.WordStatusEnum.IsWaiting;
            await AddAsync(word);
            return null;
        }

        public async Task<PaginatedList<WordResponse>> PrivateWordList(WordListRequest wordListRequest, int userId)
        {
            var query = FindBy(x => x.IsActive && x.UserId == userId && x.WordStatusEnum == Data.EntityEnum.WordStatusEnum.IsPrivate);
            var data = await PaginatedList<WordResponse>.CreateAsync(query.Select(x => new WordResponse
            {
                Id = x.Id,
                CreateDate = x.CreateDate,
                IsActive = x.IsActive,
                WordEn = x.WordEn,
                WordTr = x.WordTr,
            }), wordListRequest.Page, wordListRequest.Limit);

            return data;
        }
    }
}
