using System;

namespace FileUpload.Domain.Entities
{
    public class FileCategory
    {
        public Guid FileId { get; set; }
        public File File { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
