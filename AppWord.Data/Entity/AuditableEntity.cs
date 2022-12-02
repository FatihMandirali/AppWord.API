using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Entity
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        protected AuditableEntity()
        {
            CreateDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }
    }
}
