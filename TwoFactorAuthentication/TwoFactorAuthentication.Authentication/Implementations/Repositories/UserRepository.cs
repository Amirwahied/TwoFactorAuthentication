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
                return await sqlConnection.ExecuteAsync(nameof(StoredProcedureNames.users_login_info_ActivateUser)
                                            , new { Id = id }
                                            , commandType: CommandType.StoredProcedure
                    );
            }
        }
        public async Task<int> Enable2FA(Guid id)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.ExecuteAsync(nameof(StoredProcedureNames.users_login_info_EnableTwoFactorAuthentication)
                                           , new { Id = id }
                                           , commandType: CommandType.StoredProcedure
                   );
            }
        }

        public async Task<User_Login_Info?> GetByUsername(string username)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<User_Login_Info>(nameof(StoredProcedureNames.users_login_info_GetUserByUserName)
                                                                        , new { Username = username }
                                                                        , commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int?> CheckUserTokenExistence(string token)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<int?>(nameof(StoredProcedureNames.users_login_info_CheckUserTokenExistence)
                                                                        , new { Token = token }
                                                                        , commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> UpdateToken(Guid id, string token)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.ExecuteAsync(nameof(StoredProcedureNames.users_login_info_UpdateToken)
                                            , new { Id = id, newToken = token }
                                            , commandType: CommandType.StoredProcedure
                    );
            }
        }

        public async Task<int> UpdateAuthenticatorKey(Guid id, string manualKey)
        {
            await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();
            {
                return await sqlConnection.ExecuteAsync(nameof(StoredProcedureNames.users_login_info_UpdateAuthenticatorKey)
                                            , new { Id = id, authenticatorKey = manualKey }
                                            , commandType: CommandType.StoredProcedure
                    );
            }
        }
    }
}
