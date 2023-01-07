using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.Request
{
    public class ApplyPrivateQuizRequest
    {
        public int TrueCount { get; set; }
        public int FalseCount { get; set; }
        public List<int> FalseWordIdList { get; set; }
    }
}
