
using FileUpload.Api.Domain.Common;
using System;

namespace FileUpload.Api.Domain.Entities
{
    public class User: BaseEntity
    {
        public long UsedSpace { get; set; }
    }
}
