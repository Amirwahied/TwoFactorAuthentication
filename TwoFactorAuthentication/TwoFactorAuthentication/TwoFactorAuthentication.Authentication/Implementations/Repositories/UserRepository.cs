using System.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;
using TwoFactorAuthentication.Authentication.Models;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Core.Contracts.Repositories;
using TwoFactorAuthentication.Core.Enums;
using TwoFactorAuthentication.Core.Implementations.Repositories;

namespace TwoFactorAuthentication.Authentication.Implementations.Repositories
{
    public class UserRepository : GenericRepository<User_Login_Info>, IUserRepository
    {
        public UserRepository(ISqlConnectionFactory sqlConnection) : base(sqlConnection)
        {

        }

        public async Task<int> ActivateUser(Guid id)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return sqlConnection.Execute(StoredProcedureNames.users_login_info_ActivateUser.ToString()
                                            , new { Id = id}
                                            , commandType: CommandType.StoredProcedure
                    );
            }
        }

        public async Task<User_Login_Info?> GetByUsername(string username)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return sqlConnection.QueryFirstOrDefault<User_Login_Info>(StoredProcedureNames.users_login_info_GetUserByUserName.ToString()
                                                                        , new { Username = username}
                                                                        , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
