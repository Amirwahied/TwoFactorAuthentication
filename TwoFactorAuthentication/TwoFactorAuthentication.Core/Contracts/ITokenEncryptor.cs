using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthentication.Core.Contracts
{
    public interface ITokenEncryptor
    {
        string Encrypt(string textToEncrypt);
        string Decrypt(string textToDecrypt);
    }
}
