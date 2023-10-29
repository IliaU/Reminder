USE [Reminder]
GO
/****** Object:  StoredProcedure [io].[NodeSetStatus]    Script Date: 04.10.2023 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Погодин Илья
-- Create date: 04.10.2023
-- Description:	Получение данных из таблицы конфиг
/*
exec [io].[TaskSelect] @GuidTask='5ae28107-dd96-4294-91a2-25962fd22748', @DomainName='ISV', @HostName='ISV', @UserName='Admin', @StatusLoockMachine='False', @ClassVersion=1000000000, @PluginClass='IoSystem.Network.IoHost', @TaskProcessTyp='Monitoring'
*/
-- =============================================
ALTER PROCEDURE [io].[TaskSelect]		
	@GuidTask uniqueidentifier,
	@DomainName varchar(100),
	@HostName varchar(100),
	@UserName varchar(100),
	@StatusLoockMachine bit,
	@ClassVersion int,
	@PluginClass varchar(300),
	@TaskProcessTyp varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ErrorMessage varchar(1000), @ErrorMes nvarchar(max), @CurrentDateTime DateTime=GETDATE();
	Declare @GlobalTrace bit=0;
    
	-- 
    Declare @CurNameProcedure varchar(100) = '[io].[TaskSelect]';
    Print 'Регистрация и запуск скрипта: '+@CurNameProcedure; 
    --Declare @LogGUID table (LogGUID uniqueidentifier); 
    --Insert [ddl].[CreateMirror_Log] ([DateTime],RealStart,Script) 
    --Output inserted.[GUID] into @LogGUID(LogGUID) 
    --Values(@CurrentDateTime,GETDATE(),'[ddl].[CreateMirror_Log] @database='+coalesce(@database,'null') + ', @HistoryLogDay='+coalesce(Convert(nvarchar,@HistoryLogDay),'null')); 

	--         
    begin try 
    ---------------------- 
		------------------------------------------------ 
        Set @ErrorMessage='	0. Проверка параметров.'; Print @ErrorMessage;  
			
			if (@GuidTask is null or @DomainName is null or @HostName is null or @UserName is null or @PluginClass is null)
			begin
				Set @ErrorMes='Параметры @GuidTask, @DomainName, @HostName, @UserName, @PluginClass не могут быть пустыми.'; 
       			RaisError(@ErrorMes,12,1);         
			end
			
			If exists (Select 1 From [io].[Config] Where [ParamSpace] = 'io' and [ParamGroup] = 'Global' and [ParamName] = 'Trace' and [ValBit0]=1)
				begin
					set @GlobalTrace=1
				end
			else
				begin
					set @GlobalTrace=0;
				end

        ------------------------------------------------ 
        Set @ErrorMessage='	1. Получаем данные из таблицы io.Task.'; Print @ErrorMessage;                   

			if (OBJECT_ID('tempdb..#TmpTaskSelect') is not null) drop table #TmpTaskSelect;

			begin try
				begin transaction 

					;With T As (Select *, Min(Prioritet) over(Partition By PluginClassMethod) As PMin
							From [io].[Task] with (UPDLOCK, HOLDLOCK)
							Where (case When @TaskProcessTyp='Monitoring' then null else GuidTask end is null) -- Если мы запрашиваем не в режиме мониторинга то должны учитывать только задания никем не взятые в работу
								and @TaskProcessTyp like [TaskProcessTyp]
								and [PluginClass] = @PluginClass
								and [ClassVersion] <= @ClassVersion
								and coalesce([StatusLoockMachine], @StatusLoockMachine) = @StatusLoockMachine
								and @UserName like [UserName]
								and @HostName like [HostName]
								and @DomainName like [DomainName]
								-- Надо добавить фильтрацию по локации (Location) ноды для того чтобы на удалённых хостах можно было отключить задания в регионах например
								)
					Select [PluginClass], [ObjParamListId], [ObjInParamListId], [PluginClassMethod], [StartTimeOutSec], [Segment], [TaskProcessTyp],
						[ErrorRetryCount], [ErrorRetryTimeOutSec], [ReflexionTimeOutSec], [Prioritet], [GuidTask] into #TmpTaskSelect
					From T
					Where [Prioritet]=[PMin]
			
					-- Если вызов не с типом мониторинг то нужно сделать обновления пополям которые не пустые для того чтобы повторно не выполнялось задание на других нодах но делать мы должны по объектно чтобы один сервер начал обрабатывать один объект
					if (@TaskProcessTyp<>'Monitoring')
					begin
						;With R As (Select * , ROW_NUMBER() over (partition by [ObjParamListId] order by [PluginClassMethod]) As PRN
								From #TmpTaskSelect)
						
						update T Set GuidTask=@GuidTask
						From  [io].[Task] T
							inner join R On T.PluginClass=R.PluginClass and T.ObjParamListId = R.ObjParamListId
						Where T.PluginClass=@PluginClass
							and T.GuidTask is null;					
					end

					-- В любом случае мы должны верноуть идентификатор задания для того чтобы нода могла понять брать её это задание или нет так как она в любом случае сравнивает тот индентификатор что отправила и тот что вернула процедура это защита чтобы не взяли две ноды одно и тоже задание
					update #TmpTaskSelect Set GuidTask=@GuidTask Where GuidTask is null;

				commit transaction
			end try
			begin catch
				set @ErrorMessage=ERROR_MESSAGE(); 
				rollback transaction
				raiserror(@ErrorMes,12,1);
			end catch

		------------------------------------------------ 
        Set @ErrorMessage='	2. Получаем данные из таблицы io.Task.'; Print @ErrorMessage;

				Select *
				From #TmpTaskSelect;

        ------------------------------------------------ 
        Set @ErrorMessage='  Завершилось с успехом.'; Print @ErrorMessage; 

			if (@GlobalTrace=1)
			begin
				Insert into [io].[Log] ([DateTime], [Message], [Source], [Status])
				Values(GetDate(), @ErrorMessage, @CurNameProcedure, 'Message');
			end


            --Update [ddl].[CreateMirror_Log] Set RealEnd=GETDATE() Where [GUID]=(Select LogGUID from @LogGUID); 
    ---------------------- 
    end try 
    begin catch 
        if ERROR_NUMBER()=50000 
            Begin 
                set @ErrorMessage=ERROR_MESSAGE(); 
            End 
        Else 
            Begin 
                set @ErrorMessage='ПРОИЗОШЛА ОШИБКА НА ШАГЕ: '+ @ErrorMessage+' ('+Convert(nvarchar,ERROR_NUMBER()) + ' - '+ ERROR_MESSAGE() + ')'; 
            End 
        -- 
        Print 'Фиксация ошибки' 
        Set @ErrorMes=Convert(varchar(15),ERROR_NUMBER())+' - '+Replace(@ErrorMessage,'''',''''''); 
        -- 
        --Update [ddl].[CreateMirror_Log] Set RealEnd=GETDATE(), Error=@ErrorMes 
        --Where [GUID]=(Select LogGUID from @LogGUID); 
        -- 
		Insert into [io].[Log] ([DateTime], [Message], [Source], [Status])
		Values(GetDate(), @ErrorMes, @CurNameProcedure, 'Error');
		--
        raiserror(@ErrorMes,12,1); 
    end catch                 


END
