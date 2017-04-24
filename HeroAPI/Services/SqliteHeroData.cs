using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HeroAPI.Services
{
     /// <summary>
     /// Implementation for sqlite hero repository
     /// </summary>
    public class SqliteHeroData : IHeroData
    {
        private ApplicationDbContext _dbContext;

        public SqliteHeroData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Hero>> GetAllHeroes()
        {
            return await _dbContext.Hero
                .Select(h => h)
                .ToListAsync();
        }

        public async Task<Hero> GetHero(int id)
        {
            return await _dbContext.Hero
                .Where(h => h.HeroId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Hero> AddHero(Hero hero)
        {
           var addedHero =  _dbContext.Hero.Add(hero).Entity;
           await Commit();
           return addedHero;
        }

        public async Task DeleteHero(int id)
        {
            var hero = _dbContext.Hero.FirstOrDefault(p => p.HeroId == id);
            _dbContext.Hero.Remove(hero);
            await Commit();
        }

        public Task<int> Commit()
        {
             return _dbContext.SaveChangesAsync();
        }

    }

}