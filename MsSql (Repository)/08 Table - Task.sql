USE [Reminder]
GO

/****** Object:  Table [io].[PulBasicStatus]    Script Date: 03.04.2023 22:34:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [io].[Task](
	[ObjParamListId] [uniqueidentifier] NOT NULL,
	[PluginClass] [varchar](300) NOT NULL,
	[Location] [varchar](Max) NULL,
	[DomainName] [varchar](100) NULL,
	[HostName] [varchar](100) NULL,
	[UserName] [varchar](100) NULL,
	[StatusLoockMachine] [bit] NULL,
	[ClassVersion] [int] NOT NULL default (0),
	[StartTimeOutSec] [int] NOT NULL default (15), 
	[Segment] [varchar](1) NULL,
	[ErrorRetryCount]  [int] NOT NULL default (3),
	[ErrorRetryTimeOutSec] [int] NOT NULL default (30),
	[ReflexionTimeOutSec] [int] NOT NULL default (3600),
	[Prioritet] [int] NOT NULL default (30),
	[GuidTask] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[ObjParamListId] ASC,
	[PluginClass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, DATA_COMPRESSION = PAGE) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»дентификатор параметров дл€ которых предназначено задание и объекта с которым эти параметры св€заны' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ObjParamListId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' ласс плагина дл€ которого предназначаетс€ это задание. (аналог метода который вызываетс€ в классе)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'PluginClass'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' ‘мзическое расположение ноды в дереве с разделителем \. ѕо умолчанию % что означает любое расположение' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»м€ домена дл€ которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'DomainName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»м€ хоста дл€ которой предназначена задача' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'HostName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»м€ пользовател€ дл€ которого предназначено задание' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«адаЄтс€ статус машины в которых разрешено запускать задание если пусто значит в любых состо€ни€х' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StatusLoockMachine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ћинимальна€ верси€ ноды меньше ктоорой нельз€ брать задани€' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ClassVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ћинимальное количество секунд через которое может задача повтор€тс€ если правила срабатывани€ оказались такими же как в предыдущий раз. ѕо умолчанию 15 секунд.' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'StartTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'–еализаци€ хранилища данных' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Segment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' оличество повторов в случае сбо€ на других нодах (ѕри условии что  все оберации атомарны)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'“аймаут в секундах заданный к повторному повторению в случае сбо€ (ѕри условии что  все оберации атомарны)' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ErrorRetryTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' оличество секунд после которого считать что задание свалилось если небыло ответа от выполн€ющей ноды' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'ReflexionTimeOutSec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ѕриоритет срабатывани€ если по услови€м подходит несколько правил то срабатывает то которое имеет самый маленький приоритет что считаетс€ самым первым в списке правил. ѕо умолчанию 1000' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'Prioritet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»дентификатор задани€ который получит база чтобы зафиксировать задание за этой нодой' , @level0type=N'SCHEMA',@level0name=N'io', @level1type=N'TABLE',@level1name=N'Task', @level2type=N'COLUMN',@level2name=N'GuidTask'
GO
