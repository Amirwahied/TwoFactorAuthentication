using Dapper;
using System.Data;
using System.Data.SqlClient;
using TwoFactorAuthentication.Core.Contracts.Repositories;
using TwoFactorAuthentication.Core.Models;


namespace TwoFactorAuthentication.Core.Implementations.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ISqlConnectionFactory _sqlConnection;

        public GenericRepository(ISqlConnectionFactory sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<int> CreateAsync(T entity,string procedureName)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.ExecuteAsync(procedureName
                                            ,entity
                                            ,commandType: CommandType.StoredProcedure
                    );
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

        public Task<T?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
