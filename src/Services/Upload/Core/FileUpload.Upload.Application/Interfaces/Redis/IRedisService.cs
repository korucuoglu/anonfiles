using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Redis
{
    public interface IRedisService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object data);
        Task<bool> IsKeyAsync(string key);
        Task RemoveAsync(string key);
    }
}
