using System;
using System.Linq;
using HeroAPI.Services;

namespace HeroAPI.Data
{
    public class DbCleaner
    {
        /// <summary>
        /// Purges the Db for invalidated tokens. That is, TokenStore objects that
        /// have a false IsValid flag
        /// </summary>
        /// <param name="serviceProvider">NET core service locator</param>
        /// <param name="tokenService">Token related services</param>
        public static void RemoveInvalidatedTokens(IServiceProvider serviceProvider, JWTAuthTokenServices tokenService)
        {
            var dbContext = serviceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            
            dbContext.TokenStore.RemoveRange(dbContext.TokenStore
                .Where(t => !t.IsValid));
            
            dbContext.SaveChanges();
        }
    }
}