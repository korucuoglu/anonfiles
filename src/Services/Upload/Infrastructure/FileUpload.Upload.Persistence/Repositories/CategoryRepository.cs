using Dapper;
using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _connection;

        public CategoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Category>> GetAll(int userId)
        {
            var query = $"select * from categories where  user_id = {userId}";

            return (await _connection.QueryAsync<Category>(query)).AsList();
        }

        public async Task<Category> GetById(int id, int userId)
        {
            var query = $"select * from categories where id = {id} and user_id = {userId}";

            return await _connection.QueryFirstOrDefaultAsync<Category>(query);
        }

        public async Task<int> Save(Category category)
        {
            var query = $"insert into categories (title, created_date, user_id) values(@Title, @CreatedDate, @ApplicationUserId) returning id";

            return await _connection.ExecuteScalarAsync<int>(query, category);
        }

        public async Task<bool> Update(Category cat)
        {
            var query = $"update categories set title = @Title where id = @Id and user_id=@ApplicationUserId";
            return await _connection.ExecuteAsync(query, cat) > 0;
        }
    }
}
