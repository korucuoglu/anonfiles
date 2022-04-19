
using FileUpload.Upload.Domain.Entities;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<File>
    {
        Task<bool> AddFileWithSp(string fileName, long size, string fileKey, int userId);
        Task<bool> DeleteFileWithSp(int fileId, int userId);
    }
}
