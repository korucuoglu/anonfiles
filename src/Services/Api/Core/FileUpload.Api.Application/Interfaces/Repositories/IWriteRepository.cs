using FileUpload.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Repositories
{
    public interface IWriteRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);
    }
}
