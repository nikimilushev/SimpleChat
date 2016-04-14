USE [master]
GO

CREATE DATABASE [SimpleChat] 
GO

USE [SimpleChat]
GO

CREATE TABLE [dbo].[Message](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO

CREATE TABLE [dbo].[Session](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nickname] [nvarchar](50) NOT NULL,
	[Event] [nvarchar](50) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

GO

ALTER TABLE [dbo].[Session] ADD  CONSTRAINT [DF_Session_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
