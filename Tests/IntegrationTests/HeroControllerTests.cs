using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HeroAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace tests.HeroAPITests.IntegrationTests 
{
    [TestClass]
    public class HeroControllerTests
    {
        private TestServer _server;
        private HttpClient _client;

        [TestInitialize]
        public void TokenGenerationInitializer() 
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot("/Users/adamscarlat/Documents/Github/WebAPI-Angular-2-Quickstart-Project/Tests/")
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }
        
        [TestMethod]
        public async Task GetAllHeroesTest_ValidToken()
        {
            //Arrange 
            var token = await GetTokenHelper();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == "OK");

        }

        [TestMethod]
        public async Task GetAllHeroesTest_InvalidToken()
        {
            //Arrange 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "INVALID");

            //Act
            var response = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == "Unauthorized");

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

            System.Console.WriteLine(response.StatusCode.ToString());

            //Assert
            Assert.IsTrue(response.StatusCode.ToString() == "Unauthorized");

        }

        private async Task<string> GetTokenHelper()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TESTUSER"),
                new KeyValuePair<string, string>("password", "secret"),
            });
            var response = await _client.PostAsync("/token", content);

            var responeJson =  await response.Content.ReadAsStringAsync();
            responeJson = responeJson.ToString();

            var responseObject = JsonConvert.DeserializeObject<TokenRespone>(responeJson);
            return responseObject.AccessToken;

        }
    
    }

    public class TokenRespone
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}