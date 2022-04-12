using FileUpload.Upload.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<Category> GetById(int id, int userId);
        public Task<int> Save(Category category);
        public Task<List<Category>> GetAll(int userId);
        public Task<bool> Update(Category cat);
    }
}
