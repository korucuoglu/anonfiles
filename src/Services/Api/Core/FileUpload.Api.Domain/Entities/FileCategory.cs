using FileUpload.Api.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace FileUpload.Api.Domain.Entities
{
    public class FileCategory: BaseEntity
    {
        public string FileId { get; set; }

        [BsonIgnore]
        public File File { get; set; }

        public string CategoryId { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }
    }
}
