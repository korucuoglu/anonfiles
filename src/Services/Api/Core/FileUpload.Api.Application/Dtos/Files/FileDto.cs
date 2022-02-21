using System;

namespace FileUpload.Application.Dtos.Files
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }
    }
}
