/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[Event]    Script Date: 15-Jan-18 4:40:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Event]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Event](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[EventType] [int] NULL,
	[RoomID] [int] NOT NULL,
	[Notes] [nvarchar](150) NULL,
	[HostID] [int] NULL,
	[AttendeeID] [int] NOT NULL,
	[EventStatus] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Attendee]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Attendee] FOREIGN KEY([AttendeeID])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Attendee]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Attendee]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Host]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Host] FOREIGN KEY([HostID])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Host]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Host]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Room] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Room]
GO


