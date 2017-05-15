using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.HeroAPITests.IntegrationTests
{
    [TestClass]
    public class TokenGenerationTests : IntegrationTestsBase
    {
        [ClassInitialize()]
        public static void TokenGenerationInitializer(TestContext testContext) 
        {
            TestInitialize();
        }

        [TestMethod]
        public async Task GetToken_NoCredentials()
        {   
            var response = await _client.GetAsync(TestConfigurations.TokenAPI);
            var responseCode = response.StatusCode.ToString();

            Assert.AreEqual(TestConfigurations.BadRequestCode, responseCode);
        }

        [TestMethod]
        public async Task GetToken_ValidCredentials()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestConfigurations.TestUsername),
                new KeyValuePair<string, string>("password", TestConfigurations.TestPassword),
            });
            var response = await _client.PostAsync(TestConfigurations.TokenAPI, content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [TestMethod]
        public async Task GetToken_InvalidUsername()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "INVALID"),
                new KeyValuePair<string, string>("password", TestConfigurations.TestPassword),
            });
            var response = await _client.PostAsync(TestConfigurations.TokenAPI, content);

            Assert.AreEqual(TestConfigurations.BadRequestCode, response.StatusCode.ToString());
        }

        [TestMethod]
        public async Task GetToken_InvalidPassword()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestConfigurations.TestUsername),
                new KeyValuePair<string, string>("password", "INVALID"),
            });
            var response = await _client.PostAsync(TestConfigurations.TokenAPI, content);

            Assert.AreEqual(TestConfigurations.BadRequestCode, response.StatusCode.ToString());
        }
    }
}