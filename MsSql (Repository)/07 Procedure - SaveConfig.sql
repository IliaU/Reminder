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
-- Description:	Вставки строк в конфиг
/*
exec [io].[SaveConfig] @ParamSpace='io', @ParamGroup='ProviderMon', @ParamName='PlugInType', @ValStr0='ProviderMsSql'
*/
-- =============================================
Create PROCEDURE [io].[SaveConfig]		
	@ParamSpace varchar(300),
	@ParamGroup varchar(300),
	@ParamName varchar(300),
	@ValStr0 varchar(max) = null,
	@ValFloat0 float = null,
	@ValInt0 int = null,
	@ValBigInt0 bigint = null,
	@ValBit0 bit = null,
	@ValDate0 date = null
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ErrorMessage varchar(1000), @ErrorMes nvarchar(max), @CurrentDateTime DateTime=GETDATE();
	Declare @GlobalTrace bit=0;
    
	-- 
    Declare @CurNameProcedure varchar(100) = '[io].[SaveConfig]';
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
			
			if (@ParamSpace is null or @ParamGroup is null or @ParamName is null)
			begin
				Set @ErrorMes='Параметры @ParamSpace, @ParamGroup, @ParamName не могут быть пустыми.'; 
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
        Set @ErrorMessage='	1. Фиксируем новую строку в таблице io.Config.'; Print @ErrorMessage;                   

			if exists (Select Top 1 1 
					From [io].[Config] 
					Where [ParamSpace] = @ParamSpace
						and [ParamGroup] = @ParamGroup
						and [ParamName] = @ParamName)
				begin
					Print 'Обновление строки' 
					Update [io].[Config]
					Set [ValStr0]=@ValStr0, [ValFloat0]=@ValFloat0, [ValInt0]=@ValInt0, [ValBigInt0]=@ValBigInt0,
						[ValBit0]=@ValBit0, [ValDate0]=@ValDate0
					Where [ParamSpace] = @ParamSpace
						and [ParamGroup] = @ParamGroup
						and [ParamName] = @ParamName;
				end
			else 
				begin
					Print 'Вставка новой строки'
					insert into [io].[Config]([ParamSpace], [ParamGroup], [ParamName], [ValStr0], [ValFloat0], [ValInt0], [ValBigInt0], [ValBit0], [ValDate0])
					Values (@ParamSpace, @ParamGroup, @ParamName, @ValStr0, @ValFloat0, @ValInt0, @ValBigInt0, @ValBit0, @ValDate0);
				end

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
