using System.Data;
using System.Data.SqlClient;
using TwoFactorAuthentication.Authentication.Contracts.Repositories;
using TwoFactorAuthentication.Authentication.Models;
using TwoFactorAuthentication.Core.Contracts.Repositories;
using TwoFactorAuthentication.Core.Enums;
using TwoFactorAuthentication.Core.Implementations.Repositories;
using static Dapper.SqlMapper;

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
                                            , new { Id = id }
                                            , commandType: CommandType.StoredProcedure
                    );
            }
        }

        public async Task<User_Login_Info?> GetByUsername(string username)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return sqlConnection.QueryFirstOrDefault<User_Login_Info>(StoredProcedureNames.users_login_info_GetUserByUserName.ToString()
                                                                        , new { Username = username }
                                                                        , commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int?> CheckUserTokenExistence(string token)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return sqlConnection.QueryFirstOrDefault<int?>(StoredProcedureNames.users_login_info_CheckUserTokenExistence.ToString()
                                                                        , new { Token = token }
                                                                        , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
