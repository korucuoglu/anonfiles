using AnonFilesUpload.Api.UnitTests.DataGenerator;
using AnonFilesUpload.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnonFilesUpload.Api.UnitTests.TestSetup
{
    public class CommonTestFixture
    {
        public ApplicationDbContext Context { get; set; }

        public CommonTestFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;

            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated(); // veritabanının oluşturulduğundan emin olmak için. 
           

            Context.AddData(); // DbContext sınıfına extension metot yazdık ve burada çalıştırdık. 

            Context.SaveChanges();
        }
    }
}
