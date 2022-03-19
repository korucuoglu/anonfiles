using System;

namespace FileUpload.Upload.Domain.Common
{
    public abstract class BaseIdentity: BaseEntity
    {
        public Guid ApplicationUserId { get; set; }
    }
}
