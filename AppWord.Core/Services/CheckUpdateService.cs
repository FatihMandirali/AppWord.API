using AppWord.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppWord.Data.EntityEnum;
using AppWord.Model.Models.Options;
using AppWord.Model.Models.Request;

namespace AppWord.Core.Services
{
    public class CheckUpdateService : ICheckUpdateService
    {
        private readonly VersionSettings _versionSettings;

        public CheckUpdateService(VersionSettings versionSettings)
        {
            _versionSettings = versionSettings;
        }

        public async Task<string> CheckUpdate(CheckUpdateRequest request)
        {
            Version appVersion = new Version(request.Version);
            Version configMinor;
            Version configMajor;
            if (request.Plartform == PlartformEnum.ANDROID)
            {
                configMinor = new Version(_versionSettings.AndroidMinor);
                configMajor = new Version(_versionSettings.AndroidMajor);
            }
            else
            {
                configMinor = new Version(_versionSettings.IOSMinor);
                configMajor = new Version(_versionSettings.IOSMajor); 
            }

            if (appVersion.CompareTo(configMajor) < 0)
                return "major_update";
            if (appVersion.CompareTo(configMinor) < 0)
                return "minor_update";
            
            return null;
        }
    }
}
