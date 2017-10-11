/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/



/****** Object:  Table [dbo].[Penalty]    Script Date: 10/11/2017 11:44:40 AM ******/
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

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty]  WITH CHECK ADD  CONSTRAINT [FK_Penalty_User] FOREIGN KEY([AttendeeID])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Penalty_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Penalty]'))
ALTER TABLE [dbo].[Penalty] CHECK CONSTRAINT [FK_Penalty_User]
GO


