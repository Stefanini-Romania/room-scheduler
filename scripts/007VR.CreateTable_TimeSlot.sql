/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/



/****** Object:  Table [dbo].[TimeSlot]    Script Date: 10/11/2017 3:52:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TimeSlot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TimeSlot](
	[TimeSlotID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[RoomID] [int] NOT NULL,
	[HostID] [int] NOT NULL,
 CONSTRAINT [PK_TimeSlot] PRIMARY KEY CLUSTERED 
(
	[TimeSlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TimeSlot_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeSlot]'))
ALTER TABLE [dbo].[TimeSlot]  WITH CHECK ADD  CONSTRAINT [FK_TimeSlot_Room] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TimeSlot_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeSlot]'))
ALTER TABLE [dbo].[TimeSlot] CHECK CONSTRAINT [FK_TimeSlot_Room]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TimeSlot_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeSlot]'))
ALTER TABLE [dbo].[TimeSlot]  WITH CHECK ADD  CONSTRAINT [FK_TimeSlot_User] FOREIGN KEY([HostID])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TimeSlot_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeSlot]'))
ALTER TABLE [dbo].[TimeSlot] CHECK CONSTRAINT [FK_TimeSlot_User]
GO


