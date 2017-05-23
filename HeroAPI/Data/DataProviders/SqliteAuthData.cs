using System.Linq;
using System.Threading.Tasks;
using HeroAPI.Data.DataProviderInterfaces;
using HeroAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroAPI.Data.DataProviders
{
    public class SqliteAuthData : IAuthData
    {
        private ApplicationDbContext _dbContext;

        public SqliteAuthData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task AddToken(TokenStore tokenStoreEntity)
        {
            if( !_dbContext.Set<TokenStore>().Any(t => t.Token == tokenStoreEntity.Token))
            {
                _dbContext.TokenStore.Add(tokenStoreEntity);
            }
            await Commit();
        }

        public async Task AddToken(string token, bool isValidToken)
        {
            var tokenStoreEntity = new TokenStore();
            tokenStoreEntity.Token = token;
            tokenStoreEntity.IsValid = isValidToken;
            var tokenExpirationTime = JWTAuthTokenServices.GetTokenExpirationDateTime(token);

            if (tokenExpirationTime != 0)
                tokenStoreEntity.ExpirationTime = tokenExpirationTime;

            await AddToken(tokenStoreEntity);     
        }

        public async Task<TokenStore> GetToken(string token)
        {
           return await _dbContext.TokenStore
            .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<int> Commit()
        {
             return await _dbContext.SaveChangesAsync();
        }

    }
}