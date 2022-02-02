using AnonFilesUpload.Data.Entity;

namespace AnonFilesUpload.Api.UnitTests.DataGenerator
{
    public static class Data
    {
        public static void AddData(this ApplicationDbContext context)
        {
            context.Data.AddRange(

                new AnonFilesUpload.Data.Entity.Data{

                     MetaDataId = "1",
                     Name = "1.txt",
                     ShortUri="",
                     Size= 15,
                     UserId="",
                },

                 new AnonFilesUpload.Data.Entity.Data
                 {

                     MetaDataId = "2",
                     Name = "2.txt",
                     ShortUri = "",
                     Size = 15,
                     UserId = "",
                 }

                );
        }


    }

}





