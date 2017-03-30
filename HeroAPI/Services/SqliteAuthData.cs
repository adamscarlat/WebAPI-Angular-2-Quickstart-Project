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

        public SqliteAuthData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
            tokenStoreEntity.ExpirationTime = GetTokenExpirationDateTime(token);

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

        private ulong GetTokenExpirationDateTime(string token)
        {
            var payloadBytes = Convert.FromBase64String(token.Split('.')[1] + "=");
            var payloadStr = Encoding.UTF8.GetString(payloadBytes, 0, payloadBytes.Length);

            Console.WriteLine("Exp string: {0}", payloadStr);

            // Here, I only extract the "exp" payload property. You can extract other properties if you want.
            var exp = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL }).Exp;
            
            Console.WriteLine("Exp: {0}", exp);
            
            return exp;
        }

    }
}