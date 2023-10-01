USE [Reminder]
GO

/****** Object:  Table [io].[Config]    Script Date: 01.10.2023 20:09:46 ******/
SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [io].[Config]
(
	[ParamSpace] [varchar](300) NOT NULL,
	[ParamGroup] [varchar](300) NOT NULL,
	[ParamName] [varchar](300) NOT NULL,
	[ValStr0] [varchar](max) NULL,
	[ValFloat0] [float] NULL,
	[ValInt0] [int] NULL,
	[ValBigInt0] [bigint] NULL,
	[ValBit0] [bit] NULL,
	[ValDate0] [date] NULL,
	[ValDateTime0] [datetime] NULL,
	[ValStr1] [varchar](max) NULL,
	[ValFloat1] [float] NULL,
	[ValInt1] [int] NULL,
	[ValBigInt1] [bigint] NULL,
	[ValBit1] [bit] NULL,
	[ValDate1] [date] NULL,
	[ValDateTime1] [datetime] NULL,
 CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED 
 (
	[ParamSpace] ASC,
	[ParamGroup] ASC,
	[ParamName] ASC
 )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

GO


-- Настройка значений по умолчанию
insert into [io].[Config]([ParamSpace], [ParamGroup], [ParamName], [ValBit0])
Select 'io', 'Global', 'Trace', 0
Where not exists 
	(Select [ValBit0] From [io].[Config] Where [ParamSpace] = 'io' and [ParamGroup] = 'Global' and [ParamName] = 'Trace');