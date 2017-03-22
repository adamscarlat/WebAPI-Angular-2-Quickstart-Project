
using System.Collections.Generic;
using HeroAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeroesController : Controller
    {
        private IHeroData _heroData;

        public HeroesController(IHeroData heroData)
        {
            _heroData = heroData;
        }

        // GET api/heroes
        [HttpGet]
        [Authorize]
        public IEnumerable<Hero> Get()
        {
            var heroList = _heroData.GetAllHeroes();
            
            return heroList;
        }

        // GET api/heroes/5
        [HttpGet("{id}")]
        public Hero Get(int id)
        {
            var hero = _heroData.GetHero(id);
            return hero;
        }

        // POST api/heroes
        [HttpPost]
        [Authorize]
        public Hero Post([FromBody]HeroViewModel value)
        {
            var hero = new Hero();
            hero.HeroName = value.HeroName;

            var addedHero = _heroData.AddHero(hero);

            return addedHero;
        }

        // PUT api/heroes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]HeroViewModel value)
        {
            //var hero = _dbContext.Hero.FirstOrDefault(p => p.HeroId == id);
            var hero = _heroData.GetHero(id);

            if (hero == null)
                return;
            
            hero.HeroName = value.HeroName;
            _heroData.Commit();
        }

        // DELETE api/heroes/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           _heroData.DeleteHero(id);
        }
    }
 }