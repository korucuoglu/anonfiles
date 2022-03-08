using FileUpload.Api.Application.Interfaces.Redis;

namespace FileUpload.Api.Infrastructure.Services.Redis
{
    public class RedisSettings : IRedisSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
