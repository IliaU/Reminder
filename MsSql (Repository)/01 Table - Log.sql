USE [Reminder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [io].[Log](
	[DateTime] [datetime] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Source] [varchar](300) NOT NULL,
	[Status] [varchar](50)  NULL
) ON [PRIMARY] 
GO

ALTER TABLE [io].[Log] ADD  DEFAULT (getdate()) FOR [DateTime]
GO

-- »ндекс дл€ производительности логировани€
CREATE CLUSTERED INDEX [PK_Id_log] ON [io].[Log]
(
	[DateTime] DESC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [SPartMonth]([DateTime])
GO


