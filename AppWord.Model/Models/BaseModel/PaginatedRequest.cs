using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.BaseModel
{
    public class PaginatedRequest
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public bool IsActive { get; set; }
    }
}
