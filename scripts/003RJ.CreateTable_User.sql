/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[User]    Script Date: 15-Jan-18 2:31:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Password] [nvarchar](150) NOT NULL,
	[DepartmentID] [int] NULL,
	[FirstName] [nvarchar](150) NULL,
	[LastName] [nvarchar](150) NULL,
	[IsActive] [bit] NULL,
	[DateExpire] [datetime] NOT NULL,
	[ResetPassCode] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__User__DateExpire__6E01572D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [DateExpire]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_User_Department]') AND parent_object_id = OBJECT_ID(N'[dbo].[User]'))
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Department] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_User_Department]') AND parent_object_id = OBJECT_ID(N'[dbo].[User]'))
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Department]
GO


