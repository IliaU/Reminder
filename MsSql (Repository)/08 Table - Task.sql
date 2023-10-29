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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Идентификатор параметров для которых предназначено задание и объекта с которым эти параметры связаны' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ObjParamListId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Класс плагина для которого предназначается это задание. (аналог метода который вызывается в классе)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'PluginClass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Физическое расположение ноды в дереве с разделителем \. По умолчанию % что означает любое расположение' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя домена для которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ObjInParamListId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя домена для которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'PluginClassMethod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя домена для которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DomainName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя хоста для которой предназначена задача' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'HostName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя пользователя для которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Задаётся статус машины в которых разрешено запускать задание если пусто значит в любых состояниях' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StatusLoockMachine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Минимальная версия ноды меньше ктоорой нельзя брать задания' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ClassVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Минимальное количество секунд через которое может задача повторятся если правила срабатывания оказались такими же как в предыдущий раз. По умолчанию 15 секунд.' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StartTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Реализация хранилища данных' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Segment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип задания для того чтобы работы логики в плагинах' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'TaskProcessTyp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество повторов в случае сбоя на других нодах (При условии что  все оберации атомарны)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Таймаут в секундах заданный к повторному повторению в случае сбоя (При условии что  все оберации атомарны)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество секунд после которого считать что задание свалилось если небыло ответа от выполняющей ноды' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ReflexionTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Приоритет срабатывания если по условиям подходит несколько правил то срабатывает то которое имеет самый маленький приоритет что считается самым первым в списке правил. По умолчанию 1000' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Prioritet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Идентификатор задания который получит база чтобы зафиксировать задание за этой нодой' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'GuidTask'
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
