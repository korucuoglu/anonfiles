using AnonFilesUpload.Api.UnitTests.TestSetup;
using AnonFilesUpload.Data.Entity;
using Xunit;

namespace AnonFilesUpload.Api.UnitTests.Tests.MVC.HomeController
{
    public class HomeControllerTest : IClassFixture<CommonTestFixture>
    {
     
        [Fact]
        public void Test()
        {
           
            Assert.Equal<int>(200, 200);
        }
    }
}
