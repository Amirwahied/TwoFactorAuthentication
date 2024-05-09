using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthentication.Core.Contracts
{
    public interface IPasswordHasher
    {
        byte[] HashPasword(string password, out byte[] salt);
        bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt);
    }
}
