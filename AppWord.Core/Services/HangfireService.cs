using AppWord.Core.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Services
{
    public class HangfireService: IHangfireService
    {
        private readonly IWordService _wordService;

        public HangfireService(IWordService wordService)
        {
            _wordService = wordService;
        }

        public async Task WordOfDayUpdate()
        {
            var wordOfDays = await _wordService.FindBy(x => x.IsActive && x.WordOfDayDate != DateTime.Today).OrderBy(x => x.WordOfDayCount).Take(1).ToListAsync();
            foreach (var item in wordOfDays)
            {
                item.WordOfDayDate = DateTime.Today;
                item.WordOfDayCount = item.WordOfDayCount + 1;
                await _wordService.UpdateAsync(item, item.Id);
            }
        }
    }
}
