using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HeroAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests.HeroAPITests.IntegrationTests 
{
    [TestClass]
    public class TokenGenerationTests 
    {
        private TestServer _server;
        private HttpClient _client;

        [TestInitialize]
        public void TokenGenerationInitializer() 
        {
            System.Console.WriteLine(Directory.GetCurrentDirectory());
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot("/Users/adamscarlat/Documents/Github/WebAPI-Angular-2-Quickstart-Project/Tests/")
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [TestMethod]
        public async Task GetToken_NoCredentials()
        {   
            var response = await _client.GetAsync("/token");
            var responseCode = response.StatusCode.ToString();

            Assert.AreEqual("BadRequest", responseCode);
        }

        

    }
}