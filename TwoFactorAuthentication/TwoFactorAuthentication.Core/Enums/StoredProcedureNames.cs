namespace TwoFactorAuthentication.Core.Enums
{
	public enum StoredProcedureNames
	{
		users_login_info_CreateUser,
		users_login_info_GetUserByUserName,
		users_login_info_ActivateUser,
		users_login_info_CheckUserTokenExistence,
		users_login_info_EnableTwoFactorAuthentication,
		users_login_info_UpdateToken,
        users_login_info_UpdateAuthenticatorKey
    }
}
