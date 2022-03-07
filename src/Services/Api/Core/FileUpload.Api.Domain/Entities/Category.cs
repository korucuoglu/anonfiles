using FileUpload.Api.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FileUpload.Api.Domain.Entities
{
    public class Category: BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }

        [BsonIgnore]
        public virtual ICollection<File> Files { get; set; }
        
    }
}
