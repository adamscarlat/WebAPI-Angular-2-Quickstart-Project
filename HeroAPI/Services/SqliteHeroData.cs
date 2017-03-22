using System.Collections.Generic;
using System.Linq;
using HeroAPI.Data;

namespace HeroAPI.Services
{
    /*
        Implementation for sqlite hero repository
     */
    public class SqliteHeroData : IHeroData
    {
        private ApplicationDbContext _dbContext;

        public SqliteHeroData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Hero> GetAllHeroes()
        {
            return _dbContext.Set<Hero>().ToList();
        }

        public Hero GetHero(int id)
        {
            return _dbContext.Hero.FirstOrDefault(h => h.HeroId == id);
        }

        public Hero AddHero(Hero hero)
        {
           var addedHero =  _dbContext.Hero.Add(hero).Entity;
           Commit();
           return addedHero;
        }

        public void DeleteHero(int id)
        {
            var hero = _dbContext.Hero.FirstOrDefault(p => p.HeroId == id);
            _dbContext.Hero.Remove(hero);
            Commit();
        }

        public int Commit()
        {
             return _dbContext.SaveChanges();
        }

    }

}