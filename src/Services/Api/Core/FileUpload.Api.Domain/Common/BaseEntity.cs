using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FileUpload.Api.Domain.Common
{
    public abstract class BaseEntity
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
