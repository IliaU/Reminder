USE [Reminder]
GO
/****** Object:  StoredProcedure [io].[NodeSetStatus]    Script Date: 04.10.2023 22:05:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		������� ����
-- Create date: 04.10.2023
-- Description:	��������� ������ �� ������� ������
/*
exec [io].[SelectConfig] @ParamSpace='io', @ParamGroup='ProviderMon', @ParamName='PlugInType'
*/
-- =============================================
Create PROCEDURE [io].[SelectConfig]		
	@ParamSpace varchar(300),
	@ParamGroup varchar(300),
	@ParamName varchar(300)
AS
BEGIN
	SET NOCOUNT ON;
	Declare�@ErrorMessage�varchar(1000),�@ErrorMes�nvarchar(max),�@CurrentDateTime�DateTime=GETDATE();
	Declare @GlobalTrace bit=0;
    
	--�
� � Declare @CurNameProcedure varchar(100) = '[io].[SelectConfig]';
    Print�'����������� � ������ �������: '+@CurNameProcedure;�
� � --Declare�@LogGUID�table�(LogGUID�uniqueidentifier);�
� � --Insert�[ddl].[CreateMirror_Log]�([DateTime],RealStart,Script)�
� � --Output�inserted.[GUID]�into�@LogGUID(LogGUID)�
� � --Values(@CurrentDateTime,GETDATE(),'[ddl].[CreateMirror_Log] @database='+coalesce(@database,'null')�+�', @HistoryLogDay='+coalesce(Convert(nvarchar,@HistoryLogDay),'null'));�

	-- � � � ��
� � begin�try�
� � ----------------------�
		------------------------------------------------�
� � � � Set�@ErrorMessage='	0. �������� ����������.';�Print�@ErrorMessage;��
			
			if (@ParamSpace is null or @ParamGroup is null or @ParamName is null)
			begin
				Set�@ErrorMes='��������� @ParamSpace, @ParamGroup, @ParamName �� ����� ���� �������.';�
� � � �			RaisError(@ErrorMes,12,1);�� � � ��
			end
			
			If exists (Select 1 From [io].[Config] Where [ParamSpace] = 'io' and [ParamGroup] = 'Global' and [ParamName] = 'Trace' and [ValBit0]=1)
				begin
					set @GlobalTrace=1
				end
			else
				begin
					set @GlobalTrace=0;
				end

� � � � ------------------------------------------------�
� � � � Set�@ErrorMessage='	1. �������� ������ �� ������� io.Config.';�Print�@ErrorMessage;�� � � � � � � � ��

			Select [ParamSpace], [ParamGroup], [ParamName], [ValStr0], [ValFloat0], [ValInt0], [ValBigInt0], [ValBit0], [ValDate0]
			From [io].[Config] 
			Where [ParamSpace] like @ParamSpace
				and [ParamGroup] like @ParamGroup
				and [ParamName] like @ParamName;

� � � � ------------------------------------------------�
� � � � Set�@ErrorMessage=' ������������ � �������.';�Print�@ErrorMessage;�

			if (@GlobalTrace=1)
			begin
				Insert into [io].[Log] ([DateTime], [Message], [Source], [Status])
				Values(GetDate(), @ErrorMessage, @CurNameProcedure, 'Message');
			end


� � � � � ��--Update�[ddl].[CreateMirror_Log]�Set�RealEnd=GETDATE()�Where�[GUID]=(Select�LogGUID�from�@LogGUID);�
� � ----------------------�
� � end�try�
� ��begin�catch�
� �     if�ERROR_NUMBER()=50000�
� � � � � ��Begin�
     �  � � � ��set�@ErrorMessage=ERROR_MESSAGE();�
� � � � � ��End�
� � � � Else�
� � � � � ��Begin�
� �  � �� � � ��set�@ErrorMessage='��������� ������ �� ����: '+�@ErrorMessage+' ('+Convert(nvarchar,ERROR_NUMBER())�+�' - '+�ERROR_MESSAGE()�+�')';�
� � � � � ��End�
� � � ��--�
� � � � Print�'�������� ������'�
� � � � Set�@ErrorMes=Convert(varchar(15),ERROR_NUMBER())+' - '+Replace(@ErrorMessage,'''','''''');�
� � � ��--�
� � � ��--Update�[ddl].[CreateMirror_Log]�Set�RealEnd=GETDATE(),�Error=@ErrorMes�
� � � ��--Where�[GUID]=(Select�LogGUID�from�@LogGUID);�
� � � ��--�
		Insert into [io].[Log] ([DateTime], [Message], [Source], [Status])
		Values(GetDate(), @ErrorMes, @CurNameProcedure, 'Error');
		--
� � � ��raiserror(@ErrorMes,12,1);�
� � end�catch�� � � � � � � ��


END
