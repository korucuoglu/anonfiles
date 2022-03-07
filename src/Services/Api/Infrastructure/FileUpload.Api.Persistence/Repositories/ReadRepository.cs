using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Domain.Common;
using FileUpload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Api.Persistence.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _table;

        public ReadRepository(ApplicationDbContext context)
        {
            _table = context.Set<TEntity>();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? _table.AsNoTracking().Any()
                       : _table.AsNoTracking().Any(predicate);

        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? _table.AsQueryable()
                       : _table.Where(predicate);

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                 ? await _table.AsNoTracking().ToListAsync()
                 : await _table.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
