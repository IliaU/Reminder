USE [Reminder]
GO

/****** Object:  Table [io].[PulBasicStatus]    Script Date: 03.04.2023 22:34:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [io].[PulBasicStatus](
	[MachineName] [varchar](100) NOT NULL,
	[CustomClassTyp] [varchar](300) NOT NULL,
	[LastDateReflection] [datetime] NULL,
	[VersionPul] [varchar](50) NULL,
	[LastStatucCustom] [varchar](50) NULL,
 CONSTRAINT [PK_PulBasicStatus] PRIMARY KEY CLUSTERED 
(
	[MachineName] ASC,
	[CustomClassTyp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

GO


