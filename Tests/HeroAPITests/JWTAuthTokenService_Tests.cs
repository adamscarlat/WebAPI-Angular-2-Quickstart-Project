using HeroAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Xunit;

namespace tests.HeroAPITests
{
    /// <summary>
    /// Unit tests for the JWTAuthTokenServices 
    /// </summary>
    [TestClass]
    public class JWTAuthTokenService_Tests
    {
        private string testToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJURVNUVVNFUiIsImp0aSI6IjYyZWJlNjEyLTRlZjQtNGE0YS1iNWNmLTNiMTI1MjU2MjgzYiIsImlhdCI6MTQ5MTA4Mjc1NSwiZXhwIjoxNDkxNzAxOTU1LCJpc3MiOiJFeGFtcGxlSXNzdWVyIiwiYXVkIjoiRXhhbXBsZUF1ZGllbmNlIn0.y3J7EkFg6bW04yvIAq1A_JXqJJzCZYew1ctbxJU2lQQ";

        [TestMethod]
        public void GetTokenExpirationDateTime_Test()
        {   
            //Arrange
            ulong expectedExpTime = 1491701955;

            //Act
            var expTime = JWTAuthTokenServices.GetTokenExpirationDateTime(testToken);

            //Assert
            Assert.IsTrue(expTime == expectedExpTime);
        }

        [TestMethod]
        public void ExtractTokenFromHttpRequest_ValidTokenHeaderTest()
        {
            //Arrange
            HttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            httpRequest.Headers.Add("Authorization", string.Format("Bearer {0}", testToken));

            //Act
            var extractedToken = JWTAuthTokenServices.ExtractJWTTokenFromHttpRequest(httpRequest);

            //Assert
            Assert.AreEqual(testToken, extractedToken);
        }

        [TestMethod]
        public void ExtractJWTTokenFromHttpRequest_MissingBearer()
        {
            //Arrange
            HttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            httpRequest.Headers.Add("Authorization", testToken);

            //Act
            var extractedToken = JWTAuthTokenServices.ExtractJWTTokenFromHttpRequest(httpRequest);

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(extractedToken));
        }

        [TestMethod]
        public void ExtractJWTTokenFromHttpRequest_MissingToken()
        {
            //Arrange
            HttpRequest httpRequest = new DefaultHttpRequest(new DefaultHttpContext());

            //Act
            var extractedToken = JWTAuthTokenServices.ExtractJWTTokenFromHttpRequest(httpRequest);

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(extractedToken));
        }


    }

}