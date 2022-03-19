using FileUpload.Upload.Application.Interfaces.Redis;

namespace FileUpload.Upload.Infrastructure.Services.Redis
{
    public class RedisSettings : IRedisSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
