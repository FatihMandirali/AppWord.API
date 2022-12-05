using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.Response
{
    public class WordResponse
    {
        public string WordTr { get; set; }
        public string WordEn { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
