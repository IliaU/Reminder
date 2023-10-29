USE [Reminder]
GO

/****** Object:  Table [io].[PulBasicStatus]    Script Date: 03.04.2023 22:34:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [io].[Task](
	[PluginClass] [varchar](300) NOT NULL,
	[ObjParamListId] [uniqueidentifier] NOT NULL default(NEWID()),
	[Location] [varchar](Max) NULL,
	[ObjInParamListId] [uniqueidentifier] NULL,
	[PluginClassMethod] [varchar](300) NULL,
	[DomainName] [varchar](100) NULL,
	[HostName] [varchar](100) NULL,
	[UserName] [varchar](100) NULL,
	[StatusLoockMachine] [bit] NULL,
	[ClassVersion] [int] NOT NULL default (0),
	[StartTimeOutSec] [int] NOT NULL default (15), 
	[Segment] [varchar](1) NULL,
	[TaskProcessTyp] [varchar](50) NULL,
	[ErrorRetryCount]  [int] NOT NULL default (3),
	[ErrorRetryTimeOutSec] [int] NOT NULL default (30),
	[ReflexionTimeOutSec] [int] NOT NULL default (3600),
	[Prioritet] [int] NOT NULL default (1000),
	[GuidTask] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[ObjParamListId] ASC,
	[PluginClass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ���������� ��� ������� ������������� ������� � ������� � ������� ��� ��������� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ObjParamListId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ������� ��� �������� ��������������� ��� �������. (������ ������ ������� ���������� � ������)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'PluginClass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ������������ ���� � ������ � ������������ \. �� ��������� % ��� �������� ����� ������������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������ ��� �������� ������������� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ObjInParamListId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������ ��� �������� ������������� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'PluginClassMethod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������ ��� �������� ������������� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DomainName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ����� ��� ������� ������������� ������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'HostName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������������ ��� �������� ������������� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������� ������ ������ � ������� ��������� ��������� ������� ���� ����� ������ � ����� ����������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StatusLoockMachine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������� ������ ���� ������ ������� ������ ����� �������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ClassVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������� ���������� ������ ����� ������� ����� ������ ���������� ���� ������� ������������ ��������� ������ �� ��� � ���������� ���. �� ��������� 15 ������.' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StartTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ��������� ������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Segment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������� ��� ���� ����� ������ ������ � ��������' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskProcessTyp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� �������� � ������ ���� �� ������ ����� (��� ������� ���  ��� �������� ��������)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������� � �������� �������� � ���������� ���������� � ������ ���� (��� ������� ���  ��� �������� ��������)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ������ ����� �������� ������� ��� ������� ��������� ���� ������ ������ �� ����������� ����' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ReflexionTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������� ������������ ���� �� �������� �������� ��������� ������ �� ����������� �� ������� ����� ����� ��������� ��������� ��� ��������� ����� ������ � ������ ������. �� ��������� 1000' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Prioritet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ������� ������� ������� ���� ����� ������������� ������� �� ���� �����' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'GuidTask'
GO

insert into [io].[Task]([ObjParamListId], [PluginClass], [Location], [DomainName], [HostName], [UserName], [ClassVersion], [TaskProcessTyp])
Select Max([ValUniquei0]) As [ObjParamListId], ParamGroup As [PluginClass], '%', '%', '%', '%', 1000000000, Max([ValStr0]) As [TaskProcessTyp]
From [io].[Config]
Where ParamSpace = 'PulBasicStatus'
	and ParamGroup = 'IoSystem.Network.IoHost'
	and ParamName in ('GlobalObjId', 'TaskProcessTyp')
	and not exists  (Select 1 
					From [io].[Task] 
					Where ParamSpace = 'PulBasicStatus'
						and ParamGroup = 'IoSystem.Network.IoHost')
Group By ParamGroup;
