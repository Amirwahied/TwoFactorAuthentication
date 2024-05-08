using TwoFactorAuthentication.Contracts;
using TwoFactorAuthentication.Contracts.Repositories;
using TwoFactorAuthentication.Models;

namespace TwoFactorAuthentication.Implementations.Repositories
{
    public class UserRepository : GenericRepository<User_Login_Info>, IUserRepository
    {
        public UserRepository(ISqlConnectionFactory sqlConnection) : base(sqlConnection)
        {
        }
    }
}
