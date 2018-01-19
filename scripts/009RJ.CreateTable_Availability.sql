/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/


/****** Object:  Table [dbo].[Availability]    Script Date: 19-Jan-18 1:40:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Availability]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Availability](
	[AvailabilityId] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[AvailabilityType] [int] NOT NULL,
	[RoomId] [int] NULL,
	[HostId] [int] NOT NULL,
 CONSTRAINT [PK_Availability] PRIMARY KEY CLUSTERED 
(
	[AvailabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Availability_Host]') AND parent_object_id = OBJECT_ID(N'[dbo].[Availability]'))
ALTER TABLE [dbo].[Availability]  WITH CHECK ADD  CONSTRAINT [FK_Availability_Host] FOREIGN KEY([HostId])
REFERENCES [dbo].[User] ([UserID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Availability_Host]') AND parent_object_id = OBJECT_ID(N'[dbo].[Availability]'))
ALTER TABLE [dbo].[Availability] CHECK CONSTRAINT [FK_Availability_Host]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Availability_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Availability]'))
ALTER TABLE [dbo].[Availability]  WITH CHECK ADD  CONSTRAINT [FK_Availability_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Availability_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[Availability]'))
ALTER TABLE [dbo].[Availability] CHECK CONSTRAINT [FK_Availability_Room]
GO


