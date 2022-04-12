using FileUpload.Upload.Domain.Entities;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories.Dapper
{
    public interface ICategoryRepository: IDapperRepository<Category>
    {
        public Task<bool> Update(string title, int id);
    }
}
