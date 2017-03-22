
using System.Collections.Generic;

namespace HeroAPI.Services
{
    /*
        Repository pattern for the DB
     */
    public interface IHeroData
    {
        IEnumerable<Hero> GetAllHeroes();
        Hero GetHero(int id);
        Hero AddHero(Hero hero);
        void DeleteHero(int id);
        int Commit();
    }
}