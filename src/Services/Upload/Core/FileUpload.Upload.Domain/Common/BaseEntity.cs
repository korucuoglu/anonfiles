using System;

namespace FileUpload.Upload.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
