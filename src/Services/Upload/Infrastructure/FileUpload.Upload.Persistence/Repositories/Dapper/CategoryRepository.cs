using Dapper;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories.Dapper
{
    public class CategoryRepository : DapperRepository<Category>, ICategoryRepository
    {
        private readonly IDbConnection _connection;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CategoryRepository(IDbConnection connection, ISharedIdentityService sharedIdentityService) : base(connection)
        {
            _connection = connection;
            _sharedIdentityService = sharedIdentityService;
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            var query = $"select * from category where userId = {_sharedIdentityService.GetUserId}";

            return (await _connection.QueryAsync<Category>(query)).AsList();
        }

        public override async Task<Category> GetById(int id)
        {
            var query = $"select * from category where id = {id} and userId = {_sharedIdentityService.GetUserId}";

            return await _connection.QueryFirstOrDefaultAsync<Category>(query);
        }

        public override async Task<int> Insert(Category entity)
        {
            var query = $"insert into category (title, createdDate, userId) values(@Title, @CreatedDate, @UserId) returning id";

            return await _connection.ExecuteScalarAsync<int>(query, entity);
        }

        public override async Task<bool> Delete(int id)
        {
            var query = $"delete from category where id = {id} and userId = {_sharedIdentityService.GetUserId}";
            return await _connection.ExecuteAsync(query) > 0;
        }

        public override async Task<bool> Update(Category entity)
        {
            var query = $"update category set title = @Title where id = @Id and userId = @ApplicationUserId";

            return await _connection.ExecuteAsync(query, entity) > 0;
        }

        public async Task<bool> Update(string title, int id)
        {
            var query = $"update category set title = {title} where id = {id} and userId = {_sharedIdentityService.GetUserId}";

            return await _connection.ExecuteAsync(query) > 0;

        }
    }
}
