using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        public async Task RegisterTest_ExistingRegistration()
        {
            //Arrange            
            var formKeyValue = new Dictionary<string, string>();
            formKeyValue.Add("username", TestConfigurations.TestUsername);
            formKeyValue.Add("password", TestConfigurations.TestPassword);
            formKeyValue.Add("email", TestConfigurations.TestEmail);
            var jsonData = JsonConvert.SerializeObject(formKeyValue);

            var formContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            //act
            var httpResponse = await _client.PostAsync("api/auth/register", formContent);
            var httpResponseCode = httpResponse.StatusCode;

            //Assert
            //if redirect didnt happen then registration failed
            Assert.AreNotEqual(HttpStatusCode.Found, httpResponseCode);
        }

        
        [TestMethod]
        public async Task RegisterTest_InvalidRegistration()
        {
            //Arrange            
            var formKeyValue = new Dictionary<string, string>();
            formKeyValue.Add("username", TestConfigurations.TestUsername);
            formKeyValue.Add("password", string.Empty);
            formKeyValue.Add("email", TestConfigurations.TestEmail);
            var jsonData = JsonConvert.SerializeObject(formKeyValue);

            var formContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            //act
            var httpResponse = await _client.PostAsync("api/auth/register", formContent);

            var httpResponseCode = httpResponse.StatusCode;

            //Assert
            //if redirect didnt happen then registration failed
            Assert.AreNotEqual(HttpStatusCode.Found, httpResponseCode);
        }

        [TestMethod]
        public async Task RegisterTest_ValidRegistration()
        {
            //Arrange            
            var formKeyValue = new Dictionary<string, string>();
            formKeyValue.Add("username", "newUser");
            formKeyValue.Add("password", "Secret1!");
            formKeyValue.Add("email", "new@new.com");
            var jsonData = JsonConvert.SerializeObject(formKeyValue);

            var formContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //act
            try{
                var httpResponse = await _client.PostAsync("api/auth/register", formContent);

                var httpResponseCode = httpResponse.StatusCode;

                //Assert
                //if redirect happened then registration succeeded
                Assert.AreEqual(HttpStatusCode.Found, httpResponseCode);
            }
            finally{
                //cleanup
                RemoveUser("newUser");
            }


        }

        private async Task<dynamic> ExtractContentAsString(HttpResponseMessage httpResponse, string key)
        {
            if (httpResponse == null)
                return null;
            
            var content = await httpResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine("content: " + content);
            if (!string.IsNullOrEmpty(content))
            {
                dynamic jsonResponse = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(content));
                System.Console.WriteLine("dynamic json: " + jsonResponse);
                return jsonResponse;
            }

            return null;

            // var content = await httpResponse.Content.ReadAsStringAsync();
            // if (!string.IsNullOrEmpty(content))
            // {
            //     var responseMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            //     object message;
            //     responseMap.TryGetValue(key, out message);

            //     return message;
            // }
            // return null;
        }
    }
}