USE [Task1_TwoFactorAuthentication]
GO
/****** Object:  Table [dbo].[users_login_info]    Script Date: 5/8/2024 3:45:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_login_info](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](80) NOT NULL,
	[Password] [varbinary](68) NOT NULL,
	[Salt] [varbinary](68) NOT NULL,
	[Is_Active] [bit] NOT NULL,
	[Is_TFA_Enabled] [bit] NOT NULL,
	[TFA_LastUse] [datetime] NULL,
	[Token] [varchar](70) NOT NULL,
	[Authenticator_Key] [varchar](32) NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK__users_lo__3214EC074E96BFC0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_Ac__37A5467C]  DEFAULT ((0)) FOR [Is_Active]
GO
ALTER TABLE [dbo].[users_login_info] ADD  CONSTRAINT [DF__users_log__Is_2F__38996AB5]  DEFAULT ((0)) FOR [Is_TFA_Enabled]
GO
