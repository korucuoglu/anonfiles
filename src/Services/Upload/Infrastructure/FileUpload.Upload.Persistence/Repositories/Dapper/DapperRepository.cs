using Dapper;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories.Dapper
{
    public abstract class DapperRepository<T> : IDapperRepository<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly string tableName = typeof(T).Name.ToLower();

        protected DapperRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async virtual Task<bool> Delete(int id)
        {
            var query = $"delete from {tableName} where id = {id}";
            return await _connection.ExecuteAsync(query) > 0;
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            var query = $"select * from {tableName}";

            return (await _connection.QueryAsync<T>(query)).AsList();
        }

        public async virtual Task<T> GetById(int id)
        {
            var query = $"select * from {tableName} where id = {id}";
            return await _connection.QueryFirstOrDefaultAsync<T>(query);
        }

        public async virtual Task<int> Insert(T entity)
        {
            var columns = GetColumns();

            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));

            var query = $"insert into {tableName} ({stringOfColumns}) values ({stringOfParameters})";

            return await _connection.ExecuteScalarAsync<int>(query, entity);
        }

        public async virtual Task<bool> Update(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {tableName} set {stringOfColumns} where id = @Id";

            return await _connection.ExecuteAsync(query, entity) > 0;
        }
        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }


    }
}
