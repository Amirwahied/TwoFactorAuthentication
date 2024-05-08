using System.Data.SqlClient;

namespace TwoFactorAuthentication.Contracts
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateSqlConnection();
    }
}
