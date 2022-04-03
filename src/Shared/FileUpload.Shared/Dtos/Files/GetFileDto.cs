using System;

namespace FileUpload.Shared.Dtos.Files
{
    public class GetFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }
        public string FileKey { get; set; }

    }
}
