using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Entity
{
    public class User : AuditableEntity
    {
        public RoleEnum Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int TotalTrueCount { get; set; }
        public int TotalFalseCount { get; set; }
        public int TotalPoint { get; set; }

    }
}
