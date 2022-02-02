using AnonFilesUpload.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonFilesUpload.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity 
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
