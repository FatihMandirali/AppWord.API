using AppWord.Model.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.Request
{
    public class WordRequest
    {
        private string wordTr { get; set; }
        public string WordTr { get => wordTr.ToLower(); set => wordTr = value; }
        private string wordEn { get; set; }
        public string WordEn { get => wordEn.ToLower(); set => wordEn = value; }
        public bool IsActive { get; set; }
    }
    public class WordListRequest: PaginatedRequest
    {
    }
}
