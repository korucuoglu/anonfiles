using System;

namespace FileUpload.Domain.Common
{
    public abstract class BaseIdentity: BaseEntity
    {
        public Guid ApplicationUserId { get; set; }
    }
}
