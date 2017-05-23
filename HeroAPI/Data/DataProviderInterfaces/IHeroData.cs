
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeroAPI.Data.DataProviderInterfaces
{
     /// <summary>
     /// Repository for the DB
     /// </summary>
    public interface IHeroData
    {
        Task<IEnumerable<Hero>> GetAllHeroes();
        Task<Hero> GetHero(int id);
        Task<Hero> AddHero(Hero hero);
        Task DeleteHero(int id);
        Task<int> Commit();
    }
}