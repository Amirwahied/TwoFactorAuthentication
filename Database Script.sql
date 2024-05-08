USE [Task1_TwoFactorAuthentication]
GO
/****** Object:  Table [dbo].[users_login_info]    Script Date: 5/9/2024 1:38:56 AM ******/
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
	[Token] [varbinary](64) NOT NULL,
	[Authenticator_Key] [varchar](32) NULL,
	[Created_By] [int] NULL,
 CONSTRAINT [PK__users_lo__3214EC074E96BFC0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[users_login_info] ([Id], [Username], [Password], [Salt], [Is_Active], [Is_TFA_Enabled], [TFA_LastUse], [Token], [Authenticator_Key], [Created_By]) VALUES (N'2e30445f-006f-46e6-bba7-9d7962ee1eda', N'amir', 0x3606DCBEDA3412D4ADBE0744EE80D47CA8C79DEE0B9613E2A54CE077BEE43428C49B767EDB8885F5A0707FA16F50F4214F36D32AF5E71D3720C16DF3A949F4E7, 0x50134C1202ED7386E4FBBF53319DADC9B9A506D765ADC7846886ED3F934C4758399D0D79F64E87F7F83B27D0DCF50A7AF5B652731C1FD231D3DA31BF8D26BE4C, 0, 0, NULL, 0x97B258C3006A38F03C253DCAE3AD600C61BC7C8A9C9A5DD8C7BA16C269C4B21AB1FB1966138CCBAD0660C34E08F57493A8D106BA40EADE9CA12A031821A998AD, NULL, NULL)
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_Ac__37A5467C]  DEFAULT ((0)) FOR [Is_Active]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_2F__38996AB5]  DEFAULT ((0)) FOR [Is_TFA_Enabled]
GO
/****** Object:  StoredProcedure [dbo].[users_login_info_CreateUser]    Script Date: 5/9/2024 1:38:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[users_login_info_CreateUser]
@Id uniqueidentifier,
@Username nvarchar(80),
@Password varbinary(64),
@Salt varbinary(64),
@Token varbinary(64),
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
