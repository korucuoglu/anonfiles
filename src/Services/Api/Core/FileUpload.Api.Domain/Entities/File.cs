using FileUpload.Api.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FileUpload.Api.Domain.Entities
{
    public class File : BaseEntity
    {
        public File()
        {
            Categories = new List<Category>();
        }

        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }

        public string UserId { get; set; }

        [BsonIgnore]
        public virtual ICollection<Category> Categories { get; set; }
        
        


    }
}
