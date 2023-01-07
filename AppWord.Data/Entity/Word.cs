using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppWord.Data.Entity
{
    public class Word : AuditableEntity
    {
        public string WordTr { get; set; }
        public string WordEn { get; set; }
        public int WordOfDayCount { get; set; }
        public DateTime WordOfDayDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public WordStatusEnum WordStatusEnum { get; set; }
    }
}
