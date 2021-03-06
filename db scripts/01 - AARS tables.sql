/****** Object:  Table [dbo].[Attendance]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EventSectionID] [int] NULL,
	[MemberID] [int] NOT NULL,
	[DateTimeLogged] [datetime] NULL,
	[DateCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Community]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Community](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Remarks] [nvarchar](50) NULL,
	[UTC] [decimal](18, 2) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateCreated] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[DateModified] [datetime] NULL,
	[MaxEntities] [int] NULL,
	[MaxUsers] [int] NULL,
	[DateExpiration] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Community] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LocationID] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[EventCategoryID] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DateTimeStart] [datetime] NULL,
	[DateTimeEnd] [datetime] NULL,
	[RecurrenceID] [int] NULL,
	[AllowRegistration] [bit] NULL,
	[IsExclusive] [bit] NULL,
	[IsActive] [bit] NULL,
	[DateTimeCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateTimeModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventCategory]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[AllowMemberToLogOnMultipleEvents] [bit] NOT NULL,
	[AllowSectionOverlapping] [bit] NOT NULL,
	[DateTimeCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateTimeModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[CommunityID] [int] NULL,
 CONSTRAINT [PK_EventCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventSection]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventSection](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[EventCategoryID] [int] NOT NULL,
	[Description] [nvarchar](50) NULL,
	[DateTimeCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateTimeModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_EventTopic] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[GroupCategoryID] [int] NULL,
	[GroupLeader] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[DateModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupCategory]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[AllowMemberToJoinMultipleGroups] [bit] NOT NULL,
	[AllowLeaderToHandleMultipleGroups] [bit] NOT NULL,
	[DateTimeCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateTimeModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[CommunityID] [int] NULL,
 CONSTRAINT [PK_GroupCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupMember]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMember](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NULL,
	[MemberID] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_GroupMember] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[DateTimeCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateTimeModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[CommunityID] [int] NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookUp]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookUp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Group] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[Weight] [int] NULL,
 CONSTRAINT [LookUp_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[BirthDate] [datetime] NULL,
	[Remarks] [nvarchar](max) NULL,
	[CivilStatus] [int] NULL,
	[Gender] [int] NULL,
	[InvitedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[CommunityID] [int] NULL,
 CONSTRAINT [Member_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recurrence]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recurrence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Start] [datetime] NULL,
	[End] [datetime] NULL,
	[Pattern] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Recurrence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registrant]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registrant](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[MemberID] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Registrant] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 05/06/2018 4:44:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[CommunityID] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Attendance_0]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Attendance_0] ON [dbo].[Attendance]
(
	[EventID] ASC,
	[EventSectionID] ASC,
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Event]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Event] ON [dbo].[Event]
(
	[Name] ASC,
	[EventCategoryID] ASC,
	[DateTimeStart] ASC,
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_EventCategory]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_EventCategory] ON [dbo].[EventCategory]
(
	[Name] ASC,
	[CommunityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_EventTopic]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_EventTopic] ON [dbo].[EventSection]
(
	[Name] ASC,
	[EventCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Group]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Group] ON [dbo].[Group]
(
	[Name] ASC,
	[GroupCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_GroupCategory]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupCategory] ON [dbo].[GroupCategory]
(
	[Name] ASC,
	[CommunityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_GroupMember]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupMember] ON [dbo].[GroupMember]
(
	[GroupID] ASC,
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Location]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Location] ON [dbo].[Location]
(
	[Name] ASC,
	[CommunityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LookUp]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_LookUp] ON [dbo].[LookUp]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Member]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Member] ON [dbo].[Member]
(
	[Name] ASC,
	[CommunityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Registrant]    Script Date: 05/06/2018 4:44:22 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Registrant] ON [dbo].[Registrant]
(
	[MemberID] ASC,
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Event]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Member] FOREIGN KEY([MemberID])
REFERENCES [dbo].[Member] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Member]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_EventCategory] FOREIGN KEY([EventCategoryID])
REFERENCES [dbo].[EventCategory] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_EventCategory]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Location] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Location]
GO
ALTER TABLE [dbo].[EventSection]  WITH CHECK ADD  CONSTRAINT [FK_EventSection_EventCategory] FOREIGN KEY([EventCategoryID])
REFERENCES [dbo].[EventCategory] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventSection] CHECK CONSTRAINT [FK_EventSection_EventCategory]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_GroupCategory] FOREIGN KEY([GroupCategoryID])
REFERENCES [dbo].[GroupCategory] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_GroupCategory]
GO
ALTER TABLE [dbo].[GroupMember]  WITH CHECK ADD  CONSTRAINT [FK_GroupMember_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMember] CHECK CONSTRAINT [FK_GroupMember_Group]
GO
ALTER TABLE [dbo].[GroupMember]  WITH CHECK ADD  CONSTRAINT [FK_GroupMember_Member] FOREIGN KEY([MemberID])
REFERENCES [dbo].[Member] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMember] CHECK CONSTRAINT [FK_GroupMember_Member]
GO
ALTER TABLE [dbo].[Registrant]  WITH CHECK ADD  CONSTRAINT [FK_Registrant_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registrant] CHECK CONSTRAINT [FK_Registrant_Event]
GO
ALTER TABLE [dbo].[Registrant]  WITH CHECK ADD  CONSTRAINT [FK_Registrant_Member] FOREIGN KEY([MemberID])
REFERENCES [dbo].[Member] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registrant] CHECK CONSTRAINT [FK_Registrant_Member]
GO
