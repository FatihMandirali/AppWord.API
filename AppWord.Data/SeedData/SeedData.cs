using AppWord.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace AppWord.Data.SeedData
{
    public static class SeedData
    {
        public static async Task DatabaseMigrator(this AppWordDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();
            await SeedDataCreate(dbContext);
        }
        public static async Task SeedDataCreate(AppWordDbContext dbContext)
        {
            if (await dbContext.Users.CountAsync() > 0) return;
            var user = new User
            {
                CreateDate = DateTime.Now,
                Email = "fatih.mandirali@hotmail.com",
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Password = BC.HashPassword("123456"),
                UserName = "engineerFM",
                Role= EntityEnum.RoleEnum.Admin
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var user1 = new User
            {
                CreateDate = DateTime.Now,
                Email = "fatih.mandirali2@hotmail.com",
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Password = BC.HashPassword("123456"),
                UserName = "fm",
                Role= EntityEnum.RoleEnum.User
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.SaveChangesAsync();

            var user2 = new User
            {
                CreateDate = DateTime.Now,
                Email = "fatih.mandirali4@hotmail.com",
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Password = BC.HashPassword("123456"),
                UserName = "fm2",
                Role= EntityEnum.RoleEnum.User
            };
            await dbContext.Users.AddAsync(user2);
            await dbContext.SaveChangesAsync();

            var announcement = new Announcement
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Description = "genel",
                Title="genel",
                AnnouncementType=EntityEnum.AnnouncementEnum.IsPublic,
                StartDate= DateTime.Now,
                EndDate= DateTime.Now.AddDays(1),
                
            };
            await dbContext.Announcements.AddAsync(announcement);
            await dbContext.SaveChangesAsync();

            var announcement1 = new Announcement
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Description = "özel",
                Title="özel",
                AnnouncementType=EntityEnum.AnnouncementEnum.IsPublic,
                UserId=user2.Id,
                User=user2,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };
            await dbContext.Announcements.AddAsync(announcement1);
            await dbContext.SaveChangesAsync();

            var announcement2 = new Announcement
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Description = "subscribe",
                Title= "subscribe",
                AnnouncementType=EntityEnum.AnnouncementEnum.IsSubscriber,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };
            await dbContext.Announcements.AddAsync(announcement2);
            await dbContext.SaveChangesAsync();

            var onboarding = new Onboarding
            {
                CreateDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                ModifiedDate = DateTime.Now,
                Description = "Açıklama",
                ImageUrl = "ımage urll",
                Name = "onboardingg",
                Ordering = 2
            };

            await dbContext.Onboardings.AddAsync(onboarding);
            await dbContext.SaveChangesAsync();



        }
    }
}
