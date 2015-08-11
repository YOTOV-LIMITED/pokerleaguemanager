CREATE TABLE [dbo].[GetPlayerStatisticsDto]
(
	[DtoId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[PlayerName] VARCHAR(MAX) NOT NULL, 
    [GamesPlayed] INT NOT NULL, 
    [Winnings] INT NOT NULL, 
    [PayIn] INT NOT NULL,
	[Profit] INT NOT NULL,
	[ProfitPerGame] FLOAT NOT NULL
)
