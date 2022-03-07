using System;

namespace FileUpload.Shared.Dtos.Files
{
    public class GetFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }
    }
}
