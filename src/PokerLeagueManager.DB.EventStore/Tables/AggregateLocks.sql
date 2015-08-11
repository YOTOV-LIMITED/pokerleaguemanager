CREATE TABLE [dbo].[AggregateLocks]
(
	[AggregateId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [LockExpiry] DATETIME NOT NULL
)
