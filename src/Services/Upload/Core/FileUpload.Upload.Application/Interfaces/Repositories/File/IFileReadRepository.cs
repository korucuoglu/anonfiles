using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories
{
    public interface IFileReadRepository : IReadRepository<Domain.Entities.File>
    {
        public Task<string> GetFileKey(int fileId, int userId);
    }
}
