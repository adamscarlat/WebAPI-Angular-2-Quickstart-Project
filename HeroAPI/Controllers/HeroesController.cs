
using System.Collections.Generic;
using System.Linq;
using HeroAPI.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
 {

    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeroesController : Controller
    {
        private ApplicationDbContext _dbContext;

        public HeroesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/heroes
        [HttpGet]
        public IEnumerable<Hero> Get()
        {
            var heroList = _dbContext.Set<Hero>().ToList();
            
            return heroList;
        }

        // GET api/heroes/5
        [HttpGet("{id}")]
        public Hero Get(int id)
        {
            var hero = _dbContext.Hero.FirstOrDefault(h => h.HeroId == id);
            return hero;
        }

        // POST api/heroes
        [HttpPost]
        public Hero Post([FromBody]HeroViewModel value)
        {
            var hero = new Hero();
            hero.HeroName = value.HeroName;

            var addedHero = _dbContext.Hero.Add(hero).Entity;

            _dbContext.SaveChanges();
            return addedHero;
        }

        // PUT api/heroes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]HeroViewModel value)
        {
            var hero = _dbContext.Hero.FirstOrDefault(p => p.HeroId == id);
            
            if (hero == null)
                return;
            
            hero.HeroName = value.HeroName;
            _dbContext.SaveChanges();
            
        }

        // DELETE api/heroes/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var hero = _dbContext.Hero.FirstOrDefault(p => p.HeroId == id);
            _dbContext.Hero.Remove(hero);

            _dbContext.SaveChanges();
        }
    }
 }