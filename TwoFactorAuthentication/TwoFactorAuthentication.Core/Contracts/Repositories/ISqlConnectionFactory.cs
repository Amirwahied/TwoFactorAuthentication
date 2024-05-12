using System.Data.SqlClient;

namespace TwoFactorAuthentication.Core.Contracts.Repositories
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateSqlConnection();
    }
}
