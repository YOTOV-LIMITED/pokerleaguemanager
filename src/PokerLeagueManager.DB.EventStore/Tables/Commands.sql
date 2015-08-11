CREATE TABLE [dbo].[Commands]
(
	[CommandId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [CommandDateTime] DATETIME NOT NULL, 
    [UserName] VARCHAR(50) NOT NULL, 
    [IpAddress] VARCHAR(50) NOT NULL, 
	[CommandData] VARCHAR(MAX) NOT NULL,
    [ExceptionDetails] VARCHAR(MAX) NULL
)
