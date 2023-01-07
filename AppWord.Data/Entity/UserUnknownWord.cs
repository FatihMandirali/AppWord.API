using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Entity
{
    public class UserUnknownWord:AuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int WordId { get; set; }
        public Word Word { get; set; }
    }
}
