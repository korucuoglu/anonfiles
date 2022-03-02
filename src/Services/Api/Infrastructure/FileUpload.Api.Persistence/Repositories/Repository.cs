using FileUpload.Api.Application.Interfaces.Settings;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Domain.Common;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileUpload.Api.Application.Interfaces.Context;

namespace FileUpload.Api.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        protected readonly IMongoContext _context;

        public Repository(IDatabaseSettings databaseSettings, IMongoContext context)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.AddCommand(() =>  _collection.InsertOneAsync(entity));

            return entity;
        }


        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
             _context.AddCommand(() => _collection.InsertManyAsync(entities));
        }

        public async Task Remove(TEntity entity)
        {
            _context.AddCommand(() => _collection.DeleteOneAsync(x => x.Id == entity.Id));

        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? await _collection.Find(null).AnyAsync()
                       : await _collection.Find(predicate).AnyAsync();

        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _collection.AsQueryable().Where(predicate).AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {

            return await _collection.Find(Builders<TEntity>.Filter.Where(predicate)).ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            _context.AddCommand(() => _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity));

        }

        public async Task RemoveRange(Expression<Func<TEntity, bool>> predicate)
        {
            _context.AddCommand(() => _collection.DeleteManyAsync(predicate));
        }

        public async Task Remove(Expression<Func<TEntity, bool>> predicate)
        {
            _context.AddCommand(() => _collection.FindOneAndDeleteAsync(predicate));
        }
    }
}
