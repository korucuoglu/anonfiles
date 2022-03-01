using FileUpload.Api.Application.Interfaces.Settings;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Domain.Common;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUpload.Api.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }


        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _collection.InsertManyAsync(entities);
            return true;
        }

        public async Task Remove(TEntity entity)
        {
            await _collection.DeleteOneAsync(x => x.Id == entity.Id);
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
            await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);

        }

        public async Task RemoveRange(Expression<Func<TEntity, bool>> predicate)
        {
            await _collection.DeleteManyAsync(predicate);
        }

        public async Task Remove(Expression<Func<TEntity, bool>> predicate)
        {
            await _collection.FindOneAndDeleteAsync(predicate);
        }
    }
}
