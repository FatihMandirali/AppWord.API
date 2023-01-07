using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.EntityEnum;
using AppWord.Data.Repository;
using AppWord.Model.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class OnboardingService : Repository<Onboarding>, IOnboardingService
    {
        public OnboardingService(AppWordDbContext context) : base(context)
        {
        }

        public async Task<List<OnboardingResponse>> OnboardingList(OnboardingEnum onboardingEnum)
        {
            var onboardings = await FindBy(x => x.IsActive && !x.IsDeleted && x.OnboardingEnum == onboardingEnum).AsNoTracking()
                .Select(x => new OnboardingResponse
                {
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Name = x.Name
                })
                .ToListAsync();
            return onboardings;
        }
    }
}
