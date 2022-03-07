using FileUpload.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : BaseEntity
    {
        bool Any(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
