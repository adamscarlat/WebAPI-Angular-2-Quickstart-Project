using System.Collections.Generic;
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

        [TestMethod]
        public async Task GetToken_ValidCredentials()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TESTUSER"),
                new KeyValuePair<string, string>("password", "secret"),
            });
            var response = await _client.PostAsync("/token", content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [TestMethod]
        public async Task GetToken_InvalidUsername()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "INVALID"),
                new KeyValuePair<string, string>("password", "secret"),
            });
            var response = await _client.PostAsync("/token", content);

            Assert.AreEqual("BadRequest", response.StatusCode.ToString());
        }

        [TestMethod]
        public async Task GetToken_InvalidPassword()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TESTUSER"),
                new KeyValuePair<string, string>("password", "INVALID"),
            });
            var response = await _client.PostAsync("/token", content);

            Assert.AreEqual("BadRequest", response.StatusCode.ToString());
        }
    }
}