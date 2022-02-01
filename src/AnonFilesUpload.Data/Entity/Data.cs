namespace AnonFilesUpload.Data.Entity
{
    public class Data : BaseEntity
    {
        public string MetaDataId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ShortUri { get; set; }
        public long Size { get; set; }
    }
}
