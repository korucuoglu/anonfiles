using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories.Dapper
{
    public interface IDapperRepository<T> where T: class
    {
        public Task<T> GetById(int id);
        public Task<IEnumerable<T>> GetAll();
        public Task<int> Insert(T entity);
        public Task<bool> Delete(int id);
        public Task<bool> Update(T entity);
    }
}
