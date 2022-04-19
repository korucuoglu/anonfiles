
using FileUpload.Upload.Domain.Entities;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<File>
    {
        Task<bool> AddFileWithSp(File entity);
        Task<bool> DeleteFileWithSp(int fileId, int userId);
    }
}
