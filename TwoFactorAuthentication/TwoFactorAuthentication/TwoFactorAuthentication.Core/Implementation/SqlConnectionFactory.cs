using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TwoFactorAuthentication.Core.Contracts.Repositories;

namespace TwoFactorAuthentication.Core.Implementation
{
    public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
    {
        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("Default"));
        }
    }
}
