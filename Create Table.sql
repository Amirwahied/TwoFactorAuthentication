CREATE DATABASE  Task1_TwoFactorAuthentication
GO
USE Task1_TwoFactorAuthentication
GO
CREATE TABLE "users_login_info"
(
    "Id" UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    "Username" NVARCHAR(80) NOT NULL,
    "Password" VARBINARY(68) NOT NULL,
    "Salt" VARBINARY(68) NOT NULL,
    "Is_Active" BIT NOT NULL
        DEFAULT 0,
    "Is_2Fa_Enabled" BIT NOT NULL
        DEFAULT 0,
    "2Fa_LastUse" DATETIME,
    "Token" VARCHAR(60) NOT NULL,
    "Authenticator_Key" VARCHAR(32) NULL,
    "CreatedBy" INT,
);