using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Entity
{
    public class Announcement:AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AnnouncementEnum AnnouncementType { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
