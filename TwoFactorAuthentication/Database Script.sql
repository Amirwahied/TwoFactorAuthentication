USE [Task1_TwoFactorAuthentication]
GO
/****** Object:  Table [dbo].[users_login_info]    Script Date: 5/9/2024 4:59:05 PM ******/
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
	[Token] [nvarchar](200) NOT NULL,
	[Authenticator_Key] [varchar](32) NULL,
	[Created_By] [int] NULL,
 CONSTRAINT [PK__users_lo__3214EC074E96BFC0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By]) VALUES (N'808229c3-0bad-429b-928e-353994a9c917', N'amir4', 0x1F6A2008C82E221FF9696D9570BB8CB9CECB442A842E092C42590352185EDB56FA1974EA847F84F9956E699F34876D65B341D3F34309C10F9709943541D3EB5C, 0x0B48304B5E797F8B8EF1C11AB8A65D06A869031641268420B785D0534AD54078D8BF4A911F8954B17126B23E4E5C37EF6FACEC8F9A3B135CEDB6ECE4A3B04361, 0, 0, NULL, N'zcFsfnhM5SroRhS6QTibdTKimKhWWXo2mDv9ubG0xFYkC1yITI3nQcpjwz0rWOq1', NULL, NULL)
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By]) VALUES (N'df43ff25-bfc1-4e39-aae4-5bfe1cf4f4a5', N'amir4', 0x00C977DFDD0FDAC7FCF3F5647917322260CC8CCC897362C733AA97C4FF4A2FC4331F822F615CD72A56196781322E92A14EE5B29CB76CABA65EC09F890107FAD1, 0xE53A6EE2BDB62C3D5E1C10C83D75102D62FB4F05D7E4FF57C7D903D1E09D56C3F54A7238138E0336E5632682BB003B8FD118801EBA8D46216EB749A39D6B7084, 0, 0, NULL, N'O+LbspcIcI1xD4Bl0+7OFMsMrV2LAwstOj7G7Zu5WPIDt0grrDaW3JiTXE2cOufg', NULL, NULL)
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By]) VALUES (N'1a825356-a0dd-40d3-9984-777debd30220', N'amir5', 0x1D1831EF77658917DB4336E8D991A5B24CED6EDF6B04D409D84ED02C92A898878B15B55881DA82CE4B2B23D1ECDA99C3A306B43F58093DAA9695ECD9C7A5AB8C, 0x8A81261AF11BE124E97B61B686E6BF53BAB001D20EAB055EEE639648387A75AEA2CF7AC5CF65157D85606796482B306A701D036F4432E668A623457DDF9A1AF0, 1, 0, NULL, N'tPvxcGV0OyHizoSBO+RBm58VIZXbQtMZaVmNe153NDjmil3wXqVQR4cTxlMr17Ri', NULL, NULL)
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By]) VALUES (N'2e30445f-006f-46e6-bba7-9d7962ee1eda', N'amir', 0x3606DCBEDA3412D4ADBE0744EE80D47CA8C79DEE0B9613E2A54CE077BEE43428C49B767EDB8885F5A0707FA16F50F4214F36D32AF5E71D3720C16DF3A949F4E7, 0x50134C1202ED7386E4FBBF53319DADC9B9A506D765ADC7846886ED3F934C4758399D0D79F64E87F7F83B27D0DCF50A7AF5B652731C1FD231D3DA31BF8D26BE4C, 0, 0, NULL, N'늗썘樀┼쨽귣ౠ뱡詼骜�뫇숖쑩᪲ﮱ昙谓귋怆仃鍴톨먆鳞⪡᠃ꤡ궘', NULL, NULL)
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_Ac__37A5467C]  DEFAULT ((0)) FOR [Is_Active]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_2F__38996AB5]  DEFAULT ((0)) FOR [Is_TFA_Enabled]
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_ActivateUser]    Script Date: 5/9/2024 4:59:06 PM ******/
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
/****** Object:  StoredProcedure [dbo].[users_login_info_CreateUser]    Script Date: 5/9/2024 4:59:06 PM ******/
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
@Created_By int =null,
@Is_Active bit = 0,
@Is_TFA_Enabled bit = 0,
@TFA_LastUse datetime = null,
@Authenticator_Key datetime = null
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
		   ,Authenticator_Key)
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
		   ,@Authenticator_Key)

END
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_GetUserByUserName]    Script Date: 5/9/2024 4:59:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[users_login_info_GetUserByUserName]
@Username nvarchar(80)
as

select Id
	  ,Username
	  ,Password
	  ,Salt
	  ,Is_Active
	  ,Is_TFA_Enabled
	  ,Token 
from users_login_info

where Username = @Username
GO
