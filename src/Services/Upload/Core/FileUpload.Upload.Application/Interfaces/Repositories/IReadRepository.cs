using FileUpload.Upload.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : BaseEntity
    {
        bool Any(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true);

        Task<TEntity> FindAsync(int id, bool tracking = true);

    }
}
