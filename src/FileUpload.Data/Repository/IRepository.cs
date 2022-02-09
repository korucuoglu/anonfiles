using FileUpload.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        bool Any(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);





        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
