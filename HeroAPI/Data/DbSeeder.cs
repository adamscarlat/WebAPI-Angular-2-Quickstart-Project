using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HeroAPI.Data
{

    /// <summary>
    /// Seeds the database with initial data. This class' seed methods are called from
    /// the Startup.cs class.
    /// </summary>
    public class DbSeeder
    {

        /// <summary>
        /// Seeds the database with sample users
        /// </summary>
        /// <param name="serviceProvider">Service locator</param>
        public static void SeedDbWithSampleUsers(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            SaveSampleUser(dbContext, SampleUsers.user);
        }

        /// <summary>
        /// Save a sample user in the database if user does not exist
        /// </summary>
        /// <param name="dbContext">db context</param>
        /// <param name="user">user to be saved</param>
        public static void SaveSampleUser(ApplicationDbContext dbContext, ApplicationUser user)
        {
            if (!dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user,"secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(dbContext);
                var result = userStore.CreateAsync(user);

                dbContext.SaveChanges();
            }
        }
    }

    public static class SampleUsers
    {
        public static ApplicationUser user = new ApplicationUser
        {
            Email = "testUser@testDomain.com",
            UserName = "testUser",
            PhoneNumber = "+99910029211",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
    }
}