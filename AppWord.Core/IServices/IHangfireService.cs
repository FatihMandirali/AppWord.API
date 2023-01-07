using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.IServices
{
    public interface IHangfireService
    {
        Task WordOfDayUpdate();
    }
}
