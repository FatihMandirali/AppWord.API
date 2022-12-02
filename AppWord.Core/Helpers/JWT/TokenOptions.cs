using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Helpers.JWT
{
    public class TokenOptions
    {
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int AccessTokenExpretion { get; set; }
        public string? Key { get; set; }
    }
}
