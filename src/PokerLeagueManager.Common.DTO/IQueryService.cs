using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PokerLeagueManager.Common.DTO
{
    [ServiceContract]
    public interface IQueryService
    {
        [OperationContract]
        int GetGameCountByDate(DateTime gameDate);

        [OperationContract]
        IEnumerable<GetGamesListDto> GetGamesList();

        [OperationContract]
        GetGameResultsDto GetGameResults(Guid gameId);

        [OperationContract]
        IEnumerable<GetPlayerStatisticsDto> GetPlayerStatistics();

        [OperationContract]
        IEnumerable<GetPlayerGamesDto> GetPlayerGames(string playerName);

        [OperationContract]
        IEnumerable<GetGamesWithPlayerDto> GetGamesWithPlayer(string playerName);
    }
}
