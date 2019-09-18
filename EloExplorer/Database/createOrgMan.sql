CREATE SCHEMA [OrgMan] AUTHORIZATION [dbo]
GO

CREATE TABLE [OrgMan].[TreeItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NodeText] [nvarchar](255) NULL,
	[ParentNodeId] [int] NULL,
	[SortOrder] [int] NULL,
	[ChildrenSortBy] [tinyint] NULL,
 CONSTRAINT [PK_TreeItems] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [OrgMan].[RootPaths](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreeItemId] [int] NULL,
	[RootPath] [nvarchar](255) NULL,
 CONSTRAINT [PK_RootPaths] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[RootPaths]  WITH CHECK ADD  CONSTRAINT [FK_RootPath_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

--06.06.2019 by AW
ALTER TABLE [OrgMan].[TreeItems]
ADD
	[ChildrenSortWay] [Tinyint] NOT NULL DEFAULT ((0)),
	[Creation] [datetime] NOT NULL DEFAULT (getdate()),
	[CreationUser] [nvarchar](50) NOT NULL DEFAULT (CURRENT_USER),
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL  DEFAULT ((1)) 
GO

ALTER TABLE [OrgMan].[RootPaths]
ADD
	[Creation] [datetime] NOT NULL DEFAULT (getdate()),
	[CreationUser] [nvarchar](50) NOT NULL DEFAULT (CURRENT_USER),
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL  DEFAULT ((1)) 
GO

ALTER TABLE [OrgMan].[TreeItems]
ADD
	[FilesSortBy] [Tinyint] NOT NULL DEFAULT ((0)),
	[FilesSortWay] [Tinyint] NOT NULL DEFAULT ((0))
GO

ALTER TABLE [OrgMan].[TreeItems]
ALTER COLUMN [NodeText] [nvarchar](255) NOT NULL
GO

ALTER TABLE [OrgMan].[TreeItems]
ALTER COLUMN [ChildrenSortBy] [tinyint] NOT NULL
GO

--13.06.2019 by AW
CREATE TABLE [OrgMan].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SettingName] [nvarchar](255) NULL,
	[SettingValue] [nvarchar](255) NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[Settings] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[Settings] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[Settings] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

--01.07.2019 by AW
INSERT INTO [OrgMan].[Settings]([SettingName],[SettingValue])
     VALUES
           ('TemplateFolderName','Vorlagen')
GO

--08.07.2019 by AW
ALTER TABLE [OrgMan].[Settings]
ALTER COLUMN [SettingName] [nvarchar](255) NOT NULL
GO

CREATE TABLE [OrgMan].[AppUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserDomain] [nvarchar](255) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[DisplayName] [nvarchar](255) NOT NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_AppUser] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[AppUser] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[AppUser] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[AppUser] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_AppUser] ON [OrgMan].[AppUser]
(
	[UserDomain] ASC,
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE TABLE [OrgMan].[AppUserSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppUserId] [int] NOT NULL,
	[SettingName] [nvarchar](255) NOT NULL,
	[SettingValue] [nvarchar](255) NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_AppUserSettings] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[AppUserSettings] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[AppUserSettings] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[AppUserSettings] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[AppUserSettings]  WITH CHECK ADD  CONSTRAINT [FK_AppUserSettings_AppUser] FOREIGN KEY([AppUserId])
REFERENCES [OrgMan].[AppUser] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[AppUserSettings] CHECK CONSTRAINT [FK_AppUserSettings_AppUser]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_AppUserSettings] ON [OrgMan].[AppUserSettings]
(
	[AppUserId] ASC,
	[SettingName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Settings] ON [OrgMan].[Settings]
(
	[SettingName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--10.07.219 by AW
ALTER TABLE [OrgMan].[AppUser]
ADD [IsAdmin] [bit] NOT NULL DEFAULT (0)
GO

ALTER TABLE [OrgMan].[RootPaths]
ALTER COLUMN [TreeItemId] [int] NOT NULL
GO

CREATE TABLE [OrgMan].[TreeItemGroupRights](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreeItemId] [int] NOT NULL,
	[GroupId] [nvarchar](255) NOT NULL,
	[AccessRight] [tinyint] NOT NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_TreeItemGroupRights] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[TreeItemGroupRights] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[TreeItemGroupRights] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[TreeItemGroupRights] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[TreeItemGroupRights]  WITH CHECK ADD  CONSTRAINT [FK_TreeItemGroupRights_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_TreeItemGroupRights] ON [OrgMan].[TreeItemGroupRights]
(
	[TreeItemId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [OrgMan].[TreeItemUserRights](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreeItemId] [int] NOT NULL,
	[UserId] [nvarchar](255) NOT NULL,
	[AccessRight] [tinyint] NOT NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_TreeItemUserRights] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[TreeItemUserRights] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[TreeItemUserRights] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[TreeItemUserRights] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[TreeItemUserRights]  WITH CHECK ADD  CONSTRAINT [FK_TreeItemUserRights_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[TreeItemUserRights] CHECK CONSTRAINT [FK_TreeItemUserRights_TreeItem]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_TreeItemUserRights] ON [OrgMan].[TreeItemUserRights]
(
	[TreeItemId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--18.07.2019 by AW
CREATE TABLE [OrgMan].[Reminders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreeItemId] [int] NOT NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[ReminderDate] [datetime] NOT NULL,
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_Reminders] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[Reminders]  WITH CHECK ADD  CONSTRAINT [FK_Reminders_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[Reminders] CHECK CONSTRAINT [FK_Reminders_TreeItem]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Reminders] ON [OrgMan].[Reminders]
(
	[TreeItemId] ASC,
	[Filename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_RootPaths] ON [OrgMan].[RootPaths]
(
	[TreeItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--19.07.2019 by AW

DROP TABLE [OrgMan].[Reminders]
GO

CREATE TABLE [OrgMan].[Reminders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppUserId] [int] NOT NULL,
	[TreeItemId] [int] NOT NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[ReminderDate] [datetime] NOT NULL,
	[Done] [bit] NOT NULL DEFAULT(0),
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_Reminders] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[Reminders] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[Reminders]  WITH CHECK ADD  CONSTRAINT [FK_Reminders_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[Reminders] CHECK CONSTRAINT [FK_Reminders_TreeItem]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Reminders] ON [OrgMan].[Reminders]
(
	[AppUserId] ASC,
	[TreeItemId] ASC,
	[Filename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[Reminders]  WITH CHECK ADD  CONSTRAINT [FK_Reminders_AppUser] FOREIGN KEY([AppUserId])
REFERENCES [OrgMan].[AppUser] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[Reminders] CHECK CONSTRAINT [FK_Reminders_AppUser]
GO

--07.08.2019 by AW
CREATE TABLE [OrgMan].[TreeItemFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreeItemId] [int] NOT NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL DEFAULT (0),
	[Creation] [datetime] NOT NULL,
	[CreationUser] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[LastUpdateUser] [nvarchar](50) NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_TreeItemFiles] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [OrgMan].[TreeItemFiles] ADD  DEFAULT (getdate()) FOR [Creation]
GO

ALTER TABLE [OrgMan].[TreeItemFiles] ADD  DEFAULT (user_name()) FOR [CreationUser]
GO

ALTER TABLE [OrgMan].[TreeItemFiles] ADD  DEFAULT ((1)) FOR [RowVersion]
GO

ALTER TABLE [OrgMan].[TreeItemFiles]  WITH CHECK ADD  CONSTRAINT [FK_TreeItemFile_TreeItem] FOREIGN KEY([TreeItemId])
REFERENCES [OrgMan].[TreeItems] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [OrgMan].[TreeItemFiles] CHECK CONSTRAINT [FK_TreeItemFile_TreeItem]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_TreeItemFiles] ON [OrgMan].[TreeItemFiles]
(
	[TreeItemId] ASC,
	[Filename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--08.08.2019 by AW
INSERT INTO [OrgMan].[Settings]([SettingName],[SettingValue])
     VALUES
           ('DefaultRootPath','A:\OrgMan\TEST\Files')
GO
