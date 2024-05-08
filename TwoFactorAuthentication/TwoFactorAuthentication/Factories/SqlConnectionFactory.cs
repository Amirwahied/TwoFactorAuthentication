using System.Data.SqlClient;
using TwoFactorAuthentication.Contracts;

namespace TwoFactorAuthentication.Factories
{
    public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
    {
        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("Default"));
        }
    }
}
