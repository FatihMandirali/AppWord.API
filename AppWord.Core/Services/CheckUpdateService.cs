using AppWord.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class CheckUpdateService : ICheckUpdateService
    {

        public async Task<object> CheckUpdate()
        {
            Version appVersion = new Version(request.PlatformVersion);
            return null;
        }
    }
}
