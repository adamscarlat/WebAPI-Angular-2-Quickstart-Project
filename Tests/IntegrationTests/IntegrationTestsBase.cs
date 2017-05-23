using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HeroAPI;
using HeroAPI.Data;
using HeroAPI.Data.DataProviderInterfaces;
using HeroAPI.Data.DataProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace Tests.HeroAPITests.IntegrationTests
{
    /// <summary>
    /// Base class for the end-to-end tests
    /// </summary>
    public class IntegrationTestsBase
    {
        protected static TestServer _server;
        protected static HttpClient _client;
        protected static IHeroData _heroData;
        private static ApplicationDbContext _dbContext;
        
        /// <summary>
        /// Initialize the test server host, test client and the databse 
        /// for the use of the integration tests
        /// </summary>
        protected static void TestInitialize()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(TestConfigurations.ContentRootPath)
                .UseStartup<Startup>());
            
            var dbContext = _server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            _dbContext = dbContext;

            _client = _server.CreateClient();

            if (_heroData == null)
                _heroData = new SqliteHeroData(dbContext);
            
        }

        /// <summary>
        /// Gets a jwt token from the token generator
        /// </summary>
        /// <returns>jwt as string</returns>
        protected async Task<string> GetTokenHelper()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestConfigurations.TestUsername),
                new KeyValuePair<string, string>("password", TestConfigurations.TestPassword),
            });
            var response = await _client.PostAsync(TestConfigurations.TokenAPI, content);

            var responeJson =  await response.Content.ReadAsStringAsync();
            responeJson = responeJson.ToString();

            var responseObject = JsonConvert.DeserializeObject<TokenRespone>(responeJson);
            return responseObject.AccessToken;

        }

        protected void RemoveUser(string username)
        {
            var user = _dbContext.Set<ApplicationUser>().Where(u => u.UserName == username).FirstOrDefault();
            if (user == null)
                return;

            _dbContext.Set<ApplicationUser>().Remove(user);
            _dbContext.SaveChanges();
        }
    }
    
    /// <summary>
    /// token response container for the JSON deserializer
    /// </summary>
    public class TokenRespone
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}