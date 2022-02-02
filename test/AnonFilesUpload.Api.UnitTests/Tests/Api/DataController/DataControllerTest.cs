using AnonFilesUpload.Api.UnitTests.TestSetup;
using AnonFilesUpload.Data.Entity;
using Xunit;

namespace AnonFilesUpload.Api.UnitTests.Api.DataController
{
    public class DataControllerTest: IClassFixture<CommonTestFixture>
    {
      

        [Fact]
        public void Test1()
        {
            Assert.Equal<int>(1, 1);
        }
    }
}
