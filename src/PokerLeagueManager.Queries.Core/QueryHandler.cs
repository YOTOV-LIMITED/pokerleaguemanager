using System;
using System.Collections.Generic;
using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core
{
    public class QueryHandler : IQueryService
    {
        private IQueryDataStore _queryDataStore;

        public QueryHandler(IQueryDataStore queryDataStore)
        {
            _queryDataStore = queryDataStore;
        }

        public int GetGameCountByDate(DateTime gameDate)
        {
            return _queryDataStore.GetData<GetGameCountByDateDto>().Count(x => x.GameYear == gameDate.Year &&
                                                                               x.GameMonth == gameDate.Month &&
                                                                               x.GameDay == gameDate.Day);
        }

        public IEnumerable<GetGamesListDto> GetGamesList()
        {
            return _queryDataStore.GetData<GetGamesListDto>();
        }

        public GetGameResultsDto GetGameResults(Guid gameId)
        {
            return _queryDataStore.GetData<GetGameResultsDto>().First(x => x.GameId == gameId);
        }

        public IEnumerable<GetPlayerStatisticsDto> GetPlayerStatistics()
        {
            return _queryDataStore.GetData<GetPlayerStatisticsDto>();
        }

        public IEnumerable<GetPlayerGamesDto> GetPlayerGames(string playerName)
        {
            return _queryDataStore.GetData<GetPlayerGamesDto>().Where(g => g.PlayerName.ToUpper().Trim() == playerName.ToUpper().Trim());
        }

        public IEnumerable<GetGamesWithPlayerDto> GetGamesWithPlayer(string playerName)
        {
            return _queryDataStore.GetData<GetGamesWithPlayerDto>().Where(g => g.PlayerName.ToUpper().Trim() == playerName.ToUpper().Trim());
        }
    }
}
