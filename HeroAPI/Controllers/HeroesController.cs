using System.Collections.Generic;
using System.Threading.Tasks;
using HeroAPI.Data.DataProviderInterfaces;
using HeroAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

//TODO: return redirect from all POST requests (redirect to action) - Put, Delete left
//TODO: add integration tests for Post, Put, Delete

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
        public async Task<IEnumerable<Hero>> Get()
        {
            var heroList = await _heroData.GetAllHeroes();
            
            return heroList;
        }

        // GET api/heroes/5
        [HttpGet("{id}")]
        public async Task<Hero> Get(int id)
        {
            if (!IsIdValid(id))
                return null;
            
            return await _heroData.GetHero(id);
        }

        // POST api/heroes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HeroViewModel value)
        {
            if (!ModelState.IsValid)
            {
                var errorMap = Utilities.CreateFieldErrorDictionary(ModelState);
                var errorResponse = Utilities.CreateJsonErrorResponse(errorMap);
                return BadRequest(errorResponse);
            }

            var hero = new Hero();
            hero.HeroName = value.HeroName;
            
            var newHero = await _heroData.AddHero(hero);

            if (newHero != null)
                return RedirectToAction("Get");

            return BadRequest("Error occured while saving new hero");
        }

        // PUT api/heroes/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]HeroViewModel value)
        {
            if (!IsIdValid(id))
                return;

            if (!ModelState.IsValid)
                return;
            
            var hero = await _heroData.GetHero(id);

            if (hero == null)
                return;
            
            hero.HeroName = value.HeroName;
            await _heroData.Commit();
        }

        // DELETE api/heroes/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (!IsIdValid(id))
                return;

           await _heroData.DeleteHero(id);
        }

        private bool IsIdValid(int id)
        {
            if (id >= 0 && id < int.MaxValue)
                return true;
            
            return false;
        }

    }
 }