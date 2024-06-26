﻿using TwoFactorAuthentication.Authentication.Models;
using TwoFactorAuthentication.Core.Contracts.Repositories;


namespace TwoFactorAuthentication.Authentication.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<User_Login_Info>
    {
        Task<User_Login_Info?> GetByUsername(string username);
        Task<int> ActivateUser(Guid id);
        Task<int?> CheckUserTokenExistence(string token);
        Task<int> Enable2FA(Guid createdByUserId);
        Task<int> UpdateToken(Guid id, string token);
        Task<int> UpdateAuthenticatorKey(Guid id, string manualKey);

    }
}
