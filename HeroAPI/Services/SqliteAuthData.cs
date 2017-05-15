using System.Linq;
using System.Threading.Tasks;
using HeroAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HeroAPI.Services
{
    public class SqliteAuthData : IAuthData
    {
        private ApplicationDbContext _dbContext;
        private JWTAuthTokenServices _tokenServices;

        public SqliteAuthData(ApplicationDbContext dbContext, JWTAuthTokenServices tokenServices)
        {
            _dbContext = dbContext;
            _tokenServices = tokenServices;
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
            var tokenExpirationTime = _tokenServices.GetTokenExpirationDateTime(token);

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