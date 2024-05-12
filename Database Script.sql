USE [Task1_TwoFactorAuthentication]
GO
/****** Object:  Table [dbo].[users_login_info]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_login_info](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](80) NOT NULL,
	[Password] [varbinary](64) NOT NULL,
	[Salt] [varbinary](64) NOT NULL,
	[Is_Active] [bit] NOT NULL,
	[Is_TFA_Enabled] [bit] NOT NULL,
	[TFA_LastUse] [datetime] NULL,
	[Token] [nvarchar](64) NOT NULL,
	[Authenticator_Key] [varchar](70) NULL,
	[Created_By] [uniqueidentifier] NULL,
	[Last_SignIn_Time] [time](7) NULL,
 CONSTRAINT [PK__users_lo__3214EC074E96BFC0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By], [Last_SignIn_Time]) VALUES (N'7e554542-c1da-45d5-b605-0346556995a8', N'amir8', 0x556156D502A114A7AF43FCA86BED087FD6725A67A2F06711A9E923940A5C79A91184F5734C4F375AB5043647520C7E8C1F1BEA24D8475D8725664EC5E10A5979, 0xCB595EBBFBDA253D9DCAE71A5763CC4487721F0A4A3BA5507B66E3E86AEB9D33322682F3B86ADE57FA9989F819FF43F0EABB820544263E700B95D2C368B13475, 1, 1, NULL, N'Ur9FtrMsA2Ye32EP6cL7o16ouSb/BmJ12Y/uC4od9Cr40I9ljZkvQSY3hL7SyO+m', NULL, NULL, NULL)
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By], [Last_SignIn_Time]) VALUES (N'bd56d710-5045-4503-8101-3e0d1b58f5f4', N'amir10', 0x832C0B865AEB45C30534277810092DBC3C5073571338A0066648AC3D9CC5E9C3CA8708F7096DD02A8F2291E9BA6DCFD9A0CB2A134C6C479325A3AD6DBEA8CCD2, 0xC90E7B21AE8A74403DE5AA35A336F9ACE28FAFE277735BBD5C2C97E7D710DDD571F2D83D67F533506B05DEB75C6C18531A7524B6CF7BAE31479AC3043DF7AE21, 1, 0, NULL, N'Gwy+KCT/9blBRzANwBUu37AQMozo8ss38YqXXw2Ub/IrMtqGUbwSdYsB7Nm80FiF', NULL, N'7e554542-c1da-45d5-b605-0346556995a8', NULL)
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By], [Last_SignIn_Time]) VALUES (N'c495dd4f-4798-450e-8416-680ab5c13117', N'amir11', 0x2329D91DD62EB40B2B4A3F9312C449C225386BD5AAA9750DE0B7694EF3A5EFFE5CE2D7C53546ECB36A8372FA92E1EBA3579CBD8A045C242432145003178B343C, 0x1703BECE0FDBA395B747DD71F826EE721CA6DBCF83A7A974874FAB5248A93F2798C28F0E53639EAB24437DB4344B8EE456B9E3E8830C78CEA09B7F2857838830, 0, 0, NULL, N'KIRNA5biZUAioHT/41i61SQcFsoFeNqZKnOCxXZsyi5lX5VinEYGHdJmJpKNJ8Yq', NULL, N'7e554542-c1da-45d5-b605-0346556995a8', NULL)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UN_Username]    Script Date: 5/12/2024 6:39:28 PM ******/
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [UN_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_Ac__37A5467C]  DEFAULT ((0)) FOR [Is_Active]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_2F__38996AB5]  DEFAULT ((0)) FOR [Is_TFA_Enabled]
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_ActivateUser]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[users_login_info_ActivateUser]
@Id uniqueidentifier 
as

update users_login_info
	   set Is_Active = 1
where Id = @Id

GO
/****** Object:  StoredProcedure [dbo].[users_login_info_CheckUserTokenExistence]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[users_login_info_CheckUserTokenExistence]
@Token nvarchar(64)
as

select 1 

from users_login_info

where Token = @Token

GO
/****** Object:  StoredProcedure [dbo].[users_login_info_CreateUser]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[users_login_info_CreateUser]
@Id uniqueidentifier,
@Username nvarchar(80),
@Password varbinary(64),
@Salt varbinary(64),
@Token nvarchar(200),
@Created_By uniqueidentifier = null,
@Is_Active bit = 0,
@Is_TFA_Enabled bit = 0,
@TFA_LastUse datetime = null,
@Authenticator_Key nvarchar(70) = null,
@Last_SignIn_Time time(7) = null
as
BEGIN
INSERT INTO [dbo].[users_login_info]
           ([Id]
           ,[Username]
           ,[Password]
           ,[Salt]
           ,[Token]
           ,[Created_By]
		   ,Is_Active
		   ,Is_TFA_Enabled
		   ,TFA_LastUse
		   ,Authenticator_Key
		   ,Last_SignIn_Time)
     VALUES
           (@Id
           ,@Username
           ,@Password
           ,@Salt
           ,@Token        
           ,@Created_By
		   ,@Is_Active
		   ,@Is_TFA_Enabled
		   ,@TFA_LastUse
		   ,@Authenticator_Key
		   ,@Last_SignIn_Time)

END
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_EnableTwoFactorAuthentication]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[users_login_info_EnableTwoFactorAuthentication]
@Id uniqueidentifier 
as

update users_login_info
	   set Is_TFA_Enabled = 1
where Id = @Id
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_GetUserByUserName]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[users_login_info_GetUserByUserName]
@Username nvarchar(80)
as

--select Id
--	  ,Username
--	  ,Password
--	  ,Salt
--	  ,Is_Active
--	  ,Is_TFA_Enabled
--	  ,Token 

select *

from users_login_info

where Username = @Username
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_UpdateAuthenticatorKey]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[users_login_info_UpdateAuthenticatorKey]
@Id uniqueidentifier,
@authenticatorKey nvarchar(70)
as

update users_login_info
	   set Authenticator_Key = @authenticatorKey
where Id = @Id
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_UpdateToken]    Script Date: 5/12/2024 6:39:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[users_login_info_UpdateToken]
@Id uniqueidentifier,
@newToken nvarchar(64)
as

update users_login_info
	   set Token = @newToken
where Id = @Id
GO
