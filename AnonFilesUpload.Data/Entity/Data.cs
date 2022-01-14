namespace AnonFilesUpload.Data.Entity
{
    public class Data : BaseEntity
    {
        public string MetaDataId { get; set; }
        public string Name { get; set; }
        public string ShortUri { get; set; }
        public int Size { get; set; }
    }
}
