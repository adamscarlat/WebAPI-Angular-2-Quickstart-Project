using System.Linq;
using HeroAPI.Data;

namespace HeroAPI.Services
{
    public class SqliteAuthData : IAuthData
    {
        private ApplicationDbContext _dbContext;

        public SqliteAuthData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void AddToken(TokenStore token)
        {
            //_dbContext.TokenStore.Add(token);
            Commit();
        }

        public TokenStore GetToken(string token)
        {
           // return _dbContext.TokenStore.FirstOrDefault(t => t.Token == token);
           return null;
        }

        public int Commit()
        {
             return _dbContext.SaveChanges();
        }
    }
}