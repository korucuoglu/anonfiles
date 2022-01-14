namespace AnonFilesUpload.Data.Models
{
    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
    }

    public class UploadReturnFailModel
    {
        public bool status { get; set; }
        public Error error { get; set; }
    }

}
