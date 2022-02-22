﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool Any(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(TEntity[] entities);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(TEntity[] entities);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

       
    }
}
