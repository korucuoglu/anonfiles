namespace FileUpload.Upload.Application.Interfaces.Redis
{
    public interface IRedisSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
