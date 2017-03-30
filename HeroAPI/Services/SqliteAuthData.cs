using System;
using System.Linq;
using System.Text;
using HeroAPI.Data;
using Newtonsoft.Json;

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
        
        public void AddToken(TokenStore tokenStoreEntity)
        {
            if( !_dbContext.Set<TokenStore>().Any(t => t.Token == tokenStoreEntity.Token))
            {
                _dbContext.TokenStore.Add(tokenStoreEntity);
                Commit();
            }
        }

        public void AddToken(string token, bool isValidToken)
        {
            var tokenStoreEntity = new TokenStore();
            tokenStoreEntity.Token = token;
            tokenStoreEntity.IsValid = isValidToken;
            var tokenExpirationTime = _tokenServices.GetTokenExpirationDateTime(token);

            if (tokenExpirationTime != 0)
                tokenStoreEntity.ExpirationTime = tokenExpirationTime;

            AddToken(tokenStoreEntity);     
        }

        public TokenStore GetToken(string token)
        {
           return _dbContext.TokenStore.FirstOrDefault(t => t.Token == token);
        }

        public int Commit()
        {
             return _dbContext.SaveChanges();
        }

    }
}