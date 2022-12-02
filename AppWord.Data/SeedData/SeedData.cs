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
