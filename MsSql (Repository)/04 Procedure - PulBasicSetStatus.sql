SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ïîãîäèí Èëüÿ
-- Create date: 17.09.2023
-- Description:	Ïğîöåäóğà îáíîâëåíèÿ ñòàòóñà ïóëîâ
/*
Declare @PLastDateReflection datetime = convert(datetime,convert(varchar, '2023-04-03 23:35:59.872', 21),21);
exec [io].[PulBasicSetStatus] @MachineName='ISV', @CustomClassType='ProviderPul', @LastDateReflection=@PLastDateReflection, @VersionPul='1.0.0.0', @LastStatusCustom='Success'
*/
-- =============================================
ALTER PROCEDURE [io].[PulBasicSetStatus]		
	@MachineName varchar(100),
	@CustomClassTyp varchar(300),
	@LastDateReflection datetime,
	@VersionPul varchar(50),
	@LastStatusCustom varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ErrorMessage varchar(1000), @ErrorMes nvarchar(max), @CurrentDateTime DateTime=GETDATE();
	Declare @GlobalTrace bit=0;
    
	-- 
    Declare @CurNameProcedure varchar(100) = '[io].[PulBasicSetStatus]';
    Print 'Ğåãèñòğàöèÿ è çàïóñê ñêğèïòà: '+@CurNameProcedure; 
    --Declare @LogGUID table (LogGUID uniqueidentifier); 
    --Insert [ddl].[CreateMirror_Log] ([DateTime],RealStart,Script) 
    --Output inserted.[GUID] into @LogGUID(LogGUID) 
    --Values(@CurrentDateTime,GETDATE(),'[ddl].[CreateMirror_Log] @database='+coalesce(@database,'null') + ', @HistoryLogDay='+coalesce(Convert(nvarchar,@HistoryLogDay),'null')); 

	--         
    begin try 
    ---------------------- 
		------------------------------------------------ 
        Set @ErrorMessage='	0. Ïğîâåğêà ïàğàìåòğîâ.'; Print @ErrorMessage;  
			--Set @ErrorMes='Áàçû ñ èìåíåì: ' + @database + ' íå ñóùåñòâóåò.'; 
       		--RaisError(@ErrorMes,12,1);         
			
			If exists (Select 1 From [io].[Config] Where [ParamSpace] = 'io' and [ParamGroup] = 'Global' and [ParamName] = 'Trace' and [ValBit0]=1)
				begin
					set @GlobalTrace=1
				end
			else
				begin
					set @GlobalTrace=0;
				end

        ------------------------------------------------ 
        Set @ErrorMessage='	1. Ôèêñèğóåì ñîñòîÿíèå ïóëà.'; Print @ErrorMessage;                 

			if exists (Select Top 1 1 
					From [io].[PulBasicStatus] 
					Where [MachineName] = @MachineName
						and [CustomClassTyp] = @CustomClassTyp)
				begin
					Print 'Îáíîâëåíèå ñòğîêè' 
					Update [io].[PulBasicStatus]
					Set [LastDateReflection]=@LastDateReflection, [VersionPul]=@VersionPul, [LastStatucCustom]=@LastStatusCustom
					Where [MachineName] = @MachineName
						and [CustomClassTyp] = @CustomClassTyp;
				end
			else 
				begin
					Print 'Âñòàâêà íîâîé ñòğîêè'

					insert into [io].[PulBasicStatus]([MachineName], [CustomClassTyp], [LastDateReflection], [VersionPul], [LastStatucCustom])
					Values (@MachineName, @CustomClassTyp, @LastDateReflection, @VersionPul, @LastStatusCustom);
				end

        ------------------------------------------------ 
        Set @ErrorMessage='  Çàâåğøèëîñü ñ óñïåõîì.'; Print @ErrorMessage; 

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
                set @ErrorMessage='ÏĞÎÈÇÎØËÀ ÎØÈÁÊÀ ÍÀ ØÀÃÅ: '+ @ErrorMessage+' ('+Convert(nvarchar,ERROR_NUMBER()) + ' - '+ ERROR_MESSAGE() + ')'; 
            End 
        -- 
        Print 'Ôèêñàöèÿ îøèáêè' 
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
GO
