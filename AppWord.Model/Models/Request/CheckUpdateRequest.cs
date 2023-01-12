using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.Models.Request
{
    public class CheckUpdateRequest
    {
        public PlartformEnum Plartform { get; set; }
        public string Version { get; set; }
    }
}
