using System;

namespace FileUpload.Data.Entity.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
