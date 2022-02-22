using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            // _context.SaveChanges();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? _dbSet.Any()
                       : _dbSet.Any(predicate);

        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? _dbSet.AsQueryable()
                       : _dbSet.Where(predicate);

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                 ? await _dbSet.ToListAsync()
                 : await _dbSet.Where(predicate).ToListAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            // _context.SaveChanges();
            
        }
    }
}
