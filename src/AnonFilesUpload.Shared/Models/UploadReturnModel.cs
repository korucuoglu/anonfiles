namespace AnonFilesUpload.Shared.Models
{

    public class UploadReturnModel
    {
        public bool status { get; set; }
        public Data data { get; set; }
    }

    public class Url
    {
        public string full { get; set; }
        public string @short { get; set; }
    }

    public class Size
    {
        public long bytes { get; set; }
        public string readable { get; set; }
    }

    public class Metadata
    {
        public string id { get; set; }
        public string name { get; set; }
        public Size size { get; set; }
    }

    public class File
    {
        public Url url { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Data
    {
        public File file { get; set; }
    }

   
}
