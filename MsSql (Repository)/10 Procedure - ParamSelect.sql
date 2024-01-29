USE [Reminder]
GO
/****** Object:  StoredProcedure [io].[TaskSelect]    Script Date: 09.12.2023 22:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ïîãîäèí Èëüÿ
-- Create date: 09.12.2023
-- Description:	Ïîëó÷åíèå äàííûõ èç òàáëèöû êîíôèã
/*
exec [io].[ParamSelect] @PluginClass='IoSystem.Network.IoHost'
*/
-- =============================================
Alter PROCEDURE [io].[ParamSelect]
	@PluginClass varchar(300)
AS
BEGIN
	SET NOCOUNT ON;
	Declare @ErrorMessage varchar(1000), @ErrorMes nvarchar(max), @CurrentDateTime DateTime=GETDATE();
	Declare @GlobalTrace bit=0;
    
	-- 
    Declare @CurNameProcedure varchar(100) = '[io].[ParamSelect]';
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
			
			if (@PluginClass is null)
			begin
				Set @ErrorMes='Ïàğàìåòğû @PluginClass íå ìîãóò áûòü ïóñòûìè.'; 
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
        Set @ErrorMessage='	1. Ïîëó÷àåì äàííûå èç òàáëèöû io.Config.'; Print @ErrorMessage;                   

			if (OBJECT_ID('tempdb..#TmpParamSelect') is not null) drop table #TmpParamSelect;

			begin try

					Select * into #TmpParamSelect
					From [io].[Config]
					Where [ParamSpace] in ('io', @PluginClass)
						or ([ParamSpace]='PulBasicStatus' and [ParamGroup]=@PluginClass)
			
			end try
			begin catch
				set @ErrorMessage=ERROR_MESSAGE(); 
				raiserror(@ErrorMes,12,1);
			end catch
		
		------------------------------------------------ 
        Set @ErrorMessage='	2. Ïîëó÷àåì äàííûå èç òàáëèöû io.Param.'; Print @ErrorMessage;

				Select *
				From #TmpParamSelect
				Order By 1,2,3;

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
