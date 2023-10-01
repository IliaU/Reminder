USE [Reminder]
GO

/****** Object:  Table [dbo].[PulBasicStatus]    Script Date: 03.04.2023 22:34:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [io].[NodeStatus](
	[MachineName] [varchar](100) NOT NULL,
	[LastDateReflection] [datetime] NULL,
	[VersionNode] [varchar](50) NULL,
	[LastStatusNode] [varchar](50) NULL,
 CONSTRAINT [PK_NodeStatus] PRIMARY KEY CLUSTERED 
(
	[MachineName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

GO


