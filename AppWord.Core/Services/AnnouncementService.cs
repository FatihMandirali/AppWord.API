using AppWord.Core.IServices;
using AppWord.Data;
using AppWord.Data.Entity;
using AppWord.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppWord.Data.EntityEnum;
using AppWord.Model.Models.Response;

namespace AppWord.Core.Services
{
    public class AnnouncementService : Repository<Announcement>, IAnnouncementService
    {
        public AnnouncementService(AppWordDbContext context) : base(context)
        {
        }

        public async Task<List<AnnouncementResponse>> ActiveAnnouncementGeneralList()
        {
            var dateTimeNow = DateTime.Now;
            var announcementList = await FindBy(x => (x.IsActive
            && !x.IsDeleted
            && x.StartDate < dateTimeNow
            && x.EndDate > dateTimeNow) && 
            (x.AnnouncementType == AnnouncementEnum.IsPublic 
            || x.AnnouncementType == AnnouncementEnum.IsSubscriber 
            || x.AnnouncementType == AnnouncementEnum.IsNotSubscriber)
            ).AsNoTracking().Select(x => new AnnouncementResponse
            {
                Description = x.Description,
                Title = x.Title,
            }).ToListAsync();

            return announcementList;
        }
        public async Task<List<AnnouncementResponse>> ActiveAnnouncementListByUser(int userId)
        {
            var dateTimeNow = DateTime.Now;
            var announcementList = await FindBy(x => x.IsActive
            && !x.IsDeleted
            && x.StartDate < dateTimeNow
            && x.EndDate > dateTimeNow
            && x.UserId == userId
            && x.AnnouncementType == AnnouncementEnum.IsPrivate
            ).AsNoTracking().Select(x => new AnnouncementResponse
            {
                Description = x.Description,
                Title = x.Title,
            }).ToListAsync();

            return announcementList;
        }
    }
}
