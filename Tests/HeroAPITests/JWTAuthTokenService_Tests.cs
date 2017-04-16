using HeroAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Xunit;

namespace tests.HeroAPITests
{
    [TestClass]
    public class JWTAuthTokenService_Tests
    {
        [TestMethod]
        public void GetTokenExpirationDateTime_Tests()
        {
            var jwtService = new JWTAuthTokenServices();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJURVNUVVNFUiIsImp0aSI6IjYyZWJlNjEyLTRlZjQtNGE0YS1iNWNmLTNiMTI1MjU2MjgzYiIsImlhdCI6MTQ5MTA4Mjc1NSwiZXhwIjoxNDkxNzAxOTU1LCJpc3MiOiJFeGFtcGxlSXNzdWVyIiwiYXVkIjoiRXhhbXBsZUF1ZGllbmNlIn0.y3J7EkFg6bW04yvIAq1A_JXqJJzCZYew1ctbxJU2lQQ";
            var expTime = jwtService.GetTokenExpirationDateTime(token);
            ulong expectedExpTime = 1491701955;
            
            
            Assert.IsTrue(expTime == expectedExpTime);
        }
    }

}