using Xunit;

namespace HeroAPI.Tests
{

    //to run the tests: dotnet test
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.False(true);
        }

        [Fact]
        public void Test2()
        {
            Assert.False(false);
        }
    }
}
