using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.Request
{
    public class SuggestWordRequest
    {
        public string WordTr { get; set; }
        public string WordEn { get; set; }
    }
}
