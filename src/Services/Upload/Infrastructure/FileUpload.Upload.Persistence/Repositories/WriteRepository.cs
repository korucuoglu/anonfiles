﻿using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Common;
using FileUpload.Upload.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _table;
        private readonly ApplicationDbContext _context;

        public WriteRepository(ApplicationDbContext context)
        {
            _context = context;
            _table = context.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _table.AddAsync(entity);
            return entity;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
            return true;
        }

        public void Remove(TEntity entity)
        {
            _table.Remove(entity);
        }

        public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _table.FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                return false;
            }
            _table.Remove(entity);
            return true;
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

    }
}
