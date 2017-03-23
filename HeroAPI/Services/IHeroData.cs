
using System.Collections.Generic;

namespace HeroAPI.Services
{
     /// <summary>
     /// Repository for the DB
     /// </summary>
    public interface IHeroData
    {
        IEnumerable<Hero> GetAllHeroes();
        Hero GetHero(int id);
        Hero AddHero(Hero hero);
        void DeleteHero(int id);
        int Commit();
    }
}