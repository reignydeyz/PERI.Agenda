SET IDENTITY_INSERT [prompt].[Role] ON 
GO
INSERT [prompt].[Role] ([RoleId], [Name]) VALUES (1, N'Admin')
GO
INSERT [prompt].[Role] ([RoleId], [Name]) VALUES (2, N'User')
GO
INSERT [prompt].[Role] ([RoleId], [Name]) VALUES (3, N'Developer')
GO
SET IDENTITY_INSERT [prompt].[Role] OFF
GO
