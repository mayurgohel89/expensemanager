USE [ExpenseManager]
GO
/****** Object:  Table [dbo].[TransactionBreakup]    Script Date: 12/24/2009 16:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionBreakup]') AND type in (N'U'))
DROP TABLE [dbo].[TransactionBreakup]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 12/24/2009 16:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transaction]') AND type in (N'U'))
DROP TABLE [dbo].[Transaction]
GO
/****** Object:  Table [dbo].[User]    Script Date: 12/24/2009 16:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[UserBalance]    Script Date: 12/24/2009 16:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBalance]') AND type in (N'U'))
DROP TABLE [dbo].[UserBalance]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 12/24/2009 16:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Transaction](
	[ID] [int] NOT NULL,
	[TransactionDetails] [nchar](100) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionPayee_ID] [int] NOT NULL,
	[IsVoid] [tinyint] NOT NULL CONSTRAINT [DF_Transaction_IsVoid]  DEFAULT ((0)),
	[TransactionAmount] [float] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TransactionBreakup]    Script Date: 12/24/2009 16:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionBreakup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TransactionBreakup](
	[Transaction_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_TransactionBreakup] PRIMARY KEY CLUSTERED 
(
	[Transaction_ID] ASC,
	[User_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UserBalance]    Script Date: 12/24/2009 16:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBalance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserBalance](
	[User_ID] [int] NOT NULL,
	[IncomingBalance] [money] NOT NULL CONSTRAINT [DF_TransactionSumary_IncomingBalance]  DEFAULT (0),
	[OutgoingBalance] [money] NOT NULL CONSTRAINT [DF_TransactionSumary_OutgoingBalance]  DEFAULT (0),
	[TotalBalance] [money] NOT NULL CONSTRAINT [DF_TransactionSumary_TotalBalance]  DEFAULT (0),
 CONSTRAINT [PK_UserBalance] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[User]    Script Date: 12/24/2009 16:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Users_active]  DEFAULT (1),
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'IsActive'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This value is set to 0 when any user is removed from the system' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'IsActive'
GO
/****** Object:  Trigger [ti_UpdateBalance]    Script Date: 12/24/2009 16:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ti_UpdateBalance]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[ti_UpdateBalance] ON [dbo].[TransactionBreakup] 
AFTER INSERT
AS

Declare @SQL nvarchar(400)
Declare @param nvarchar(400)
Declare @userId integer
Declare @recentValueInserted money
Declare @InBal money
Declare @OutBal money
Declare @TotalBal money


--Prcedure Imlementation Start

select [User_ID], Amount into #myTable from inserted

set @param = N''@userId as integer output, @recentValueInserted as money output''
set @SQL = N''SELECT @userId=[User_ID], @recentValueInserted=Amount FROM #myTable''
exec sp_executeSQL @SQL, @param, @userId output, @recentValueInserted output


set @param = N''@InBal as money output, @OutBal as money output, @TotalBal as money output, @userId as integer''
set @SQL = N''SELECT @InBal=IncomingBalance, @OutBal=OutgoingBalance, @TotalBal=TotalBalance FROM UserBalance WHERE [User_ID] = @userId''
exec sp_executeSQL @SQL, @param, @InBal output, @OutBal output, @TotalBal output, @userId

IF ( @recentValueInserted > 0 )

begin 
set @InBal = @InBal + @recentValueInserted
end

ELSE

begin 
set @OutBal = @OutBal + @recentValueInserted
end

Set @TotalBal = @TotalBal + @recentValueInserted

UPDATE UserBalance SET IncomingBalance = @InBal, OutgoingBalance = @OutBal, TotalBalance  = @TotalBal  WHERE [User_ID] = @userId


'
GO
