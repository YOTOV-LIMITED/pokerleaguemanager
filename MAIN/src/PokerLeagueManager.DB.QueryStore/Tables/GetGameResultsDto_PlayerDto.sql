CREATE TABLE [dbo].[GetGameResultsDto_PlayerDto]
(
	[DtoId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[GetGameResultsDto_DtoId] UNIQUEIDENTIFIER NOT NULL,
	[PlayerName] VARCHAR(MAX) NOT NULL, 
    [Placing] INT NOT NULL, 
    [Winnings] INT NOT NULL, 
    [PayIn] INT NOT NULL, 
    CONSTRAINT [FK_PlayerDto_GetGameResultsDto] FOREIGN KEY ([GetGameResultsDto_DtoId]) REFERENCES [GetGameResultsDto]([DtoId]) 
)
