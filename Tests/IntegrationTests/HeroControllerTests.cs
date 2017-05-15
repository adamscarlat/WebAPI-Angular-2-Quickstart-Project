using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tests.HeroAPITests.IntegrationTests
{
    /// <summary>
    /// End-to-end tests focused on the HeroController 
    /// </summary>
    [TestClass]
    public class HeroControllerTests : IntegrationTestsBase
    {
        [ClassInitialize()]
        public static void TokenGenerationInitializer(TestContext testContext) 
        {
            TestInitialize();
        }
        
        [TestMethod]
        public async Task GetAllHeroesTest_ValidToken()
        {
            //Arrange 
            var heroName = "TestHero";
            var token = await GetTokenHelper();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _heroData.AddHero(new Hero(){ HeroName = heroName });

            //Act
            var response = await _client.GetAsync("/api/heroes");
            var content = await response.Content.ReadAsStringAsync();
            var heroList = JsonConvert.DeserializeObject<List<Hero>>(content);
            var hero = heroList.Where(h => h.HeroName == heroName).FirstOrDefault();

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == TestConfigurations.OkCode);
            Assert.IsTrue(hero.HeroName == heroName);

            //Cleanup
            _heroData.DeleteHero(hero.HeroId);
                
        }

        [TestMethod]
        public async Task GetAllHeroesTest_InvalidToken()
        {
            //Arrange 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "INVALID");

            //Act
            var response = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == TestConfigurations.UnauthorizedCode);

        }

        [TestMethod]
        public async Task GetAllHeroesTest_InvalidatedToken()
        {
            //Arrange 
            var token = await GetTokenHelper();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            await _client.GetAsync("/api/auth/logout");
            var response = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == TestConfigurations.UnauthorizedCode);
        }
    
    }
}