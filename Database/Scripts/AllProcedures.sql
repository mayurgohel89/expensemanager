USE [ExpenseManager]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddSharedCost]    Script Date: 12/18/2009 18:45:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddSharedCost]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AddSharedCost]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddTransaction]    Script Date: 12/18/2009 18:45:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AddTransaction]
GO
/****** Object:  StoredProcedure [dbo].[sp_TransactionDetails]    Script Date: 12/18/2009 18:45:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TransactionDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TransactionDetails]
GO
/****** Object:  StoredProcedure [dbo].[sp_PopulateGridSumary]    Script Date: 12/18/2009 18:45:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_PopulateGridSumary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_PopulateGridSumary]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetActiveUsers]    Script Date: 12/18/2009 18:45:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetActiveUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetActiveUsers]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserBalance]    Script Date: 12/18/2009 18:45:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUserBalance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetUserBalance]
GO
/****** Object:  StoredProcedure [dbo].[sp_DeactivateUser]    Script Date: 12/18/2009 18:45:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeactivateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeactivateUser]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddUser]    Script Date: 12/18/2009 18:45:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AddUser]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNewUser]    Script Date: 12/18/2009 18:45:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddNewUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AddNewUser]
GO
/****** Object:  StoredProcedure [dbo].[sp_TransactionDetails]    Script Date: 12/18/2009 18:45:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TransactionDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_TransactionDetails] 

@userId as integer,
@showPositiveTransactions as bit

AS

IF( @showPositiveTransactions = 1) 

BEGIN 

SELECT [Transaction].TransactionDate as [DATE], TransactionBreakup.Amount As [Amount CREDITED To Your Account], [Transaction].TransactionDetails as [DESCRIPTION] 
FROM 
TransactionBreakup INNER JOIN [Transaction]
ON
TransactionBreakup.Transaction_ID = [Transaction].ID
WHERE
( TransactionBreakup.Amount > 0 AND TransactionBreakup.[User_Id] = @userId )

END


ELSE

BEGIN

SELECT 
[Transaction].TransactionDate as [DATE], 
TransactionBreakup.Amount As [DEDUCTION From Your Account], 
[User].[UserName] As [PAID BY], 
[Transaction].TransactionDetails as [DESCRIPTION] 

FROM
TransactionBreakup 
INNER JOIN [Transaction] ON
TransactionBreakup.Transaction_ID = [Transaction].Id
 INNER JOIN [User] ON
 [Transaction].TransactionPayee_ID = [User].Id

WHERE
TransactionBreakup.Amount < 0 AND TransactionBreakup.[User_Id] = @userId

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddTransaction]    Script Date: 12/18/2009 18:45:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddTransaction]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_AddTransaction] 

-- Exposed Variable Declaration
    @payeeId as integer,
	@details as nvarchar(100),
	@newTransactionId  as integer output
AS

-- Local Variable Declarations

Declare @SQL nvarchar(400)
Declare @param nvarchar(400)
Declare @MaxTransactionId integer

--Prcedure Implementation Start


set @param = N''	@MaxTransactionId as integer output''
set @SQL = N''SELECT @MaxTransactionId = MAX ( ID ) FROM [Transaction]''
exec sp_executeSQL @SQL, @param, @MaxTransactionId output

if @MaxTransactionId = null
begin
set @MaxTransactionId = 0
end

set @newTransactionId = @MaxTransactionId + 1
INSERT INTO [Transaction] VALUES ( @newTransactionId, @details, getDate(), @payeeId)

--SP written above is an example of dynamic sql i.e. sp_executesql
--otherwise this could have been simply achived by first inserting record and then setting  
--@MaxTransactionId to @@identity.' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddSharedCost]    Script Date: 12/18/2009 18:45:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddSharedCost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_AddSharedCost] 

-- Exposed Variable Declaration
	@transactionId as integer,
	@userId            as integer,
	@amount          as float
	
AS

--Prcedure Imlementation Start
INSERT INTO [TransactionBreakup] VALUES  ( @transactionId  ,  @userId  , @amount )



' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PopulateGridSumary]    Script Date: 12/18/2009 18:45:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_PopulateGridSumary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_PopulateGridSumary] AS

SELECT 
	
	UserName AS [USER], 
	CASE IsActive WHEN 1 THEN ''YES'' ELSE ''NO'' END AS [ACTIVE],
	IncomingBalance AS [POSITIVE DEPOSIT], 
	OutgoingBalance AS [CREDIT TAKEN], 
	TotalBalance AS [BALANCE],
	StartDate AS [ACCOUNT OPENED],
	EndDate AS [ACCOUNT CLOSED],
	[User].ID AS [USER ID]

FROM [User] WITH(NOLOCK) 
	LEFT JOIN UserBalance WITH(NOLOCK) ON UserBalance.[User_ID] = [User].ID


ORDER BY [User].IsActive DESC, [User].ID' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNewUser]    Script Date: 12/18/2009 18:45:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddNewUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_AddNewUser] 
(
	@userName VARCHAR(15)
)
AS

IF @userName = null
RETURN


DECLARE @newUserName as VARCHAR(20)
DECLARE @count AS INT
DECLARE @postFix AS INT


SET @newUserName = @userName
SET @count = 1
SET @postFix = 1

WHILE ( @count != 0 )
BEGIN
	SELECT @count = COUNT(UserName) FROM [User] WHERE UserName = @newUserName AND IsActive = 1
	IF @count > 0 
	BEGIN
		SET @newUserName = @userName + '' # '' + CAST(@postFix AS CHAR)
		SET @postFix = @postFix + 1
	END
END

INSERT INTO [User] (UserName, IsActive, StartDate) VALUES ( @newUserName, 1, GetDate())
INSERT INTO UserBalance ([User_ID], IncomingBalance, OutgoingBalance, TotalBalance) VALUES (@@IDENTITY, 0 , 0, 0)' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddUser]    Script Date: 12/18/2009 18:45:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_AddUser] 
(
	@userName VARCHAR(15)
)
AS

IF @userName = null
RETURN


DECLARE @newUserName as VARCHAR(20)
DECLARE @count AS INT
DECLARE @postFix AS INT


SET @newUserName = @userName
SET @count = 1
SET @postFix = 1

WHILE ( @count != 0 )
BEGIN
	SELECT @count = COUNT(UserName) FROM [User] WHERE UserName = @newUserName AND IsActive = 1
	IF @count > 0 
	BEGIN
		SET @newUserName = @userName + '' # '' + CAST(@postFix AS CHAR)
		SET @postFix = @postFix + 1
	END
END

INSERT INTO dbo.[User] (UserName, IsActive, StartDate) VALUES ( @newUserName, 1, GetDate())
INSERT INTO UserBalance ([User_ID], IncomingBalance, OutgoingBalance, TotalBalance) VALUES (@@IDENTITY, 0 , 0, 0)' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserBalance]    Script Date: 12/18/2009 18:45:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUserBalance]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetUserBalance] 
(
	@userId int
)
AS

if @userId = null
return

SELECT TotalBalance AS [BALANCE] FROM UserBalance WITH(NOLOCK) WHERE UserBalance.[User_ID] = @userId' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeactivateUser]    Script Date: 12/18/2009 18:45:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeactivateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_DeactivateUser] 
(
	@userId int
)
AS

if @userId = null
return

UPDATE [User] SET IsActive = 0, EndDate = getDate() WHERE ID = @userId' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetActiveUsers]    Script Date: 12/18/2009 18:45:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetActiveUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetActiveUsers] AS

SELECT ID, [UserName] from [User] WHERE IsActive = 1  ORDER BY ID' 
END
GO
