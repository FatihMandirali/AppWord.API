using AppWord.Data.EntityEnum;
using AppWord.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IOnboardingService
    {
        Task<List<OnboardingResponse>> OnboardingList(OnboardingEnum onboardingEnum);
    }
}
