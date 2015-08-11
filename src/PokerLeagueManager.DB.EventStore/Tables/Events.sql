CREATE TABLE [dbo].[Events]
(
	[EventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [EventTimestamp] TIMESTAMP NOT NULL, 
    [EventDateTime] DATETIME NOT NULL, 
    [CommandId] UNIQUEIDENTIFIER NOT NULL, 
    [AggregateId] UNIQUEIDENTIFIER NOT NULL, 
    [EventType] VARCHAR(MAX) NOT NULL, 
    [EventData] VARCHAR(MAX) NOT NULL, 
    [Published] BIT NOT NULL
)
