using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.HeroAPITests.IntegrationTests
{
    /// <summary>
    /// End-to-end tests focused on the AuthController 
    /// </summary>
    [TestClass]
    public class AuthenticationControllerTests : IntegrationTestsBase
    {
        [ClassInitialize()]
        public static void TokenGenerationInitializer(TestContext testContext) 
        {
            TestInitialize();
        }

        [TestMethod]
        public async Task LogoutTest_LogoutValidToken()
        {
            //Arrange 
            var token = await GetTokenHelper();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var logoutResponse = await _client.GetAsync("/api/auth/logout");
            var APIresponse = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(logoutResponse.StatusCode.ToString() == TestConfigurations.OkCode);
            Assert.IsTrue(APIresponse.StatusCode.ToString() == TestConfigurations.UnauthorizedCode);
        }

        [TestMethod]
        public async Task LogoutTest_LogoutNoToken()
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            //Act
            var logoutResponse = await _client.GetAsync("/api/auth/logout");
            var APIresponse = await _client.GetAsync("/api/heroes");

            //Assert
            Assert.IsTrue(logoutResponse.StatusCode.ToString() == TestConfigurations.OkCode);
            Assert.IsTrue(APIresponse.StatusCode.ToString() == TestConfigurations.UnauthorizedCode);
        }

    }
}