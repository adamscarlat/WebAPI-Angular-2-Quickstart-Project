using System.Collections.Generic;
using HeroAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{

    /// <summary>
    /// Main data API of the application
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class HeroesController : Controller
    {
        private IHeroData _heroData;

        public HeroesController(IHeroData heroData)
        {
            _heroData = heroData;
        }

        // GET api/heroes
        [HttpGet]
        public IEnumerable<Hero> Get()
        {
            var heroList = _heroData.GetAllHeroes();
            
            return heroList;
        }

        // GET api/heroes/5
        [HttpGet("{id}")]
        public Hero Get(int id)
        {
            if (!IsIdValid(id))
                return null;
            
            var hero = _heroData.GetHero(id);
            return hero;
        }

        // POST api/heroes
        [HttpPost]
        public Hero Post([FromBody]HeroViewModel value)
        {
            if (!ModelState.IsValid)
                return null;

            var hero = new Hero();

            hero.HeroName = value.HeroName;

            var addedHero = _heroData.AddHero(hero);

            return addedHero;
        }

        // PUT api/heroes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]HeroViewModel value)
        {
            if (!IsIdValid(id))
                return;

            if (!ModelState.IsValid)
                return;
            
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
            if (!IsIdValid(id))
                return;

           _heroData.DeleteHero(id);
        }

        private bool IsIdValid(int id)
        {
            if (id >= 0 && id < int.MaxValue)
                return true;
            
            return false;
        }

    }
 }