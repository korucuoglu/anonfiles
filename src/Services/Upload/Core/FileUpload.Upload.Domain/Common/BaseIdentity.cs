using System;

namespace FileUpload.Upload.Domain.Common
{
    public abstract class BaseIdentity: BaseEntity
    {
        public int ApplicationUserId { get; set; }
    }
}
