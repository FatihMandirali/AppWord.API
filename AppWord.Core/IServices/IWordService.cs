using AppWord.Data.Entity;
using AppWord.Data.Repository;
using AppWord.Model.Models.BaseModel;
using AppWord.Model.Models.Request;
using AppWord.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IWordService: IRepository<Word>
    {
        Task CreateWordList(List<WordRequest> wordRequests, int adminId);
        Task<string> CreateWord(WordRequest wordRequest, int adminId);
        Task<PaginatedList<WordResponse>> List(WordListRequest wordListRequest);
        Task<WordResponse> Detail(int id);
        Task<string> Update(WordRequest wordRequest, int id);
        Task<string> Delete(int id);
        Task<List<string>> ListAll();
        Task<List<WordResponse>> WordOfDay();
        Task<string> SuggestWord(SuggestWordRequest suggestWordRequest, int id);
        Task<string> PrivateWord(SuggestWordRequest suggestWordRequest, int id);
        Task<PaginatedList<WordResponse>> PrivateWordList(WordListRequest wordListRequest, int userId);
    }
}
