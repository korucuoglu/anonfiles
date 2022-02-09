using FileUpload.Data.Entity;

namespace FileUpload.Api.UnitTests.DataGenerator
{
    public static class Data
    {
        public static void AddData(this ApplicationDbContext context)
        {
            context.Files.AddRange(

                new FileUpload.Data.Entity.File
                {

                    Id = "1",
                    FileName = "1.txt",
                    Size = 15,
                    ApplicationUserId = "",
                },

                 new FileUpload.Data.Entity.File
                 {
                     Id = "1",
                     FileName = "1.txt",
                     Size = 15,
                     ApplicationUserId = "",
                 }

                );
        }


    }

}





