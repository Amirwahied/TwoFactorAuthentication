using Dapper;
using System.Data;
using System.Data.SqlClient;
using TwoFactorAuthentication.Contracts;
using TwoFactorAuthentication.Contracts.Repositories;
using TwoFactorAuthentication.Models;

namespace TwoFactorAuthentication.Implementations.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ISqlConnectionFactory _sqlConnection;

        public GenericRepository(ISqlConnectionFactory sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<int> CreateAsync(T entity)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                var parameters = entity;
                return sqlConnection.Execute("users_login_info_CreateUser", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
