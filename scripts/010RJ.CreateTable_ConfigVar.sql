/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2012 (11.0.3000)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[ConfigVar]    Script Date: 15-Jan-18 2:32:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigVar]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigVar](
	[VarId] [int] NOT NULL,
	[VarName] [nvarchar](50) NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_ConfigVar] PRIMARY KEY CLUSTERED 
(
	[VarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


