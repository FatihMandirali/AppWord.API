using AppWord.Data.EntityEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Data.Entity
{
    public class Onboarding : AuditableEntity
    {
        public OnboardingEnum OnboardingEnum { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Ordering { get; set; }
    }
}
