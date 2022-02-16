using FileUpload.Data.Entity;
using System;

namespace FileUpload.Api.UnitTests.DataGenerator
{
    public static class Data
    {
        public static void AddData(this ApplicationDbContext context)
        {
            context.Files.AddRange(

                new FileUpload.Data.Entity.File
                {

                    Id = Guid.NewGuid(),
                    FileName = "1.txt",
                    Size = 15,
                    ApplicationUserId = Guid.NewGuid(),
                },

                 new FileUpload.Data.Entity.File
                 {
                     Id = Guid.NewGuid(),
                     FileName = "1.txt",
                     Size = 15,
                     ApplicationUserId = Guid.NewGuid(),
                 }

                );;
        }


    }

}





