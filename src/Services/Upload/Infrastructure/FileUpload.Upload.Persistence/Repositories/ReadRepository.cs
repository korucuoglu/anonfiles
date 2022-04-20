using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Common;
using FileUpload.Upload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _table;

        public ReadRepository(ApplicationDbContext context)
        {
            _table = context.Set<TEntity>();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true)
        {

            var query = _table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate);

        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true)
        {
            var query = _table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return query.Any(predicate);

        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true)
        {
            var query = _table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return query.Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true)
        {
            var query = _table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();


            return predicate == null
                 ? await query.ToListAsync()
                 : await query.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FindAsync(int id, bool tracking = true)
        {
            return tracking
                 ? await _table.FindAsync(id)
                 : await _table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
