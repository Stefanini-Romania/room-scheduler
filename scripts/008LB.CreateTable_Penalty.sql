/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[Penalty]    Script Date: 27-Nov-17 3:06:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Penalty]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Penalty](
	[PenaltyID] [int] IDENTITY(1,1) NOT NULL,
	[AttendeeID] [int] NOT NULL,
	[EventID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[RoomId] [int] NULL,
 CONSTRAINT [PK_Penalty] PRIMARY KEY CLUSTERED 
(
	[PenaltyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_Event]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty]  WITH CHECK ADD  CONSTRAINT [FK_Penalty_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([EventID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_Event]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty] CHECK CONSTRAINT [FK_Penalty_Event]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty]  WITH CHECK ADD  CONSTRAINT [FK_Penalty_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty] CHECK CONSTRAINT [FK_Penalty_Room]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty]  WITH CHECK ADD  CONSTRAINT [FK_Penalty_User] FOREIGN KEY([AttendeeID])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty] CHECK CONSTRAINT [FK_Penalty_User]
GO


