﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Helpers.JWT
{
    public class AccessToken
    {
        public string? Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
