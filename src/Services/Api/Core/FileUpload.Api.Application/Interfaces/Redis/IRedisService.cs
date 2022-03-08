using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Redis
{
    public interface IRedisService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object data);
        Task<bool> IsKeyAsync(string key);
        Task<bool> RemoveAsync(string key);
    }
}
