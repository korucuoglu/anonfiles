using FileUpload.Api.UnitTests.DataGenerator;
using FileUpload.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace FileUpload.Api.UnitTests.TestSetup
{
    public class CommonTestFixture
    {
        public ApplicationDbContext Context { get; set; }
        public HttpClient Client { get; set; }

        public CommonTestFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated(); // veritabanının oluşturulduğundan emin olmak için. 
            Context.AddData(); // DbContext sınıfına extension metot yazdık ve burada çalıştırdık. 
            Context.SaveChanges();

            Client = new HttpClient();
        }
    }
}
