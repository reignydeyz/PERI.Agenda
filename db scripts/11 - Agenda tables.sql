/****** Object:  Table [prompt].[EndUser]    Script Date: 09/06/2018 8:16:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prompt].[EndUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[PasswordHash] [nvarchar](128) NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordExpiry] [datetime] NULL,
	[LastSessionId] [varchar](50) NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChanged] [datetime] NULL,
	[FailedPasswordCount] [int] NOT NULL,
	[LastFailedPasswordAttempt] [datetime] NULL,
	[ConfirmationCode] [varchar](50) NULL,
	[ConfirmationExpiry] [datetime] NULL,
	[DateConfirmed] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateInactive] [datetime] NULL,
 CONSTRAINT [PK__EndUser__1788CC4CE6A91100] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [prompt].[Role]    Script Date: 09/06/2018 8:16:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prompt].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_EndUser]    Script Date: 09/06/2018 8:16:38 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_EndUser] ON [prompt].[EndUser]
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [prompt].[EndUser]  WITH CHECK ADD  CONSTRAINT [FK_EndUser_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [prompt].[EndUser] CHECK CONSTRAINT [FK_EndUser_Member]
GO
ALTER TABLE [prompt].[EndUser]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [prompt].[Role] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [prompt].[EndUser] CHECK CONSTRAINT [FK_User_Role]
GO
