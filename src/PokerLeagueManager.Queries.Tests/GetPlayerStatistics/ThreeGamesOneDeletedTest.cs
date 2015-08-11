using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests.GetPlayerStatistics
{
    [TestClass]
    public class ThreeGamesOneDeletedTest : BaseQueryTest
    {
        private Guid _gameId1 = Guid.NewGuid();
        private Guid _gameId2 = Guid.NewGuid();
        private Guid _gameId3 = Guid.NewGuid();

        private string _player1 = "Dylan";
        private string _player2 = "Ryan";
        private string _player3 = "Chris";
        private string _player4 = "Jeff";

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId1, GameDate = DateTime.Now };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player1, Placing = 1, Winnings = 100, PayIn = 20 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player2, Placing = 2, Winnings = 20, PayIn = 50 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player3, Placing = 3, Winnings = 0, PayIn = 20 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player4, Placing = 4, Winnings = 0, PayIn = 30 };

            yield return new GameCreatedEvent() { AggregateId = _gameId2, GameDate = DateTime.Now };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player4, Placing = 1, Winnings = 50, PayIn = 20 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player1, Placing = 2, Winnings = 10, PayIn = 20 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player3, Placing = 3, Winnings = 0, PayIn = 20 };

            yield return new GameCreatedEvent() { AggregateId = _gameId3, GameDate = DateTime.Now };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId3, PlayerName = _player3, Placing = 1, Winnings = 60, PayIn = 40 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId3, PlayerName = _player2, Placing = 2, Winnings = 0, PayIn = 20 };

            yield return new GameDeletedEvent() { AggregateId = _gameId2 };
        }

        [TestMethod]
        public void GetPlayerStatistics_ThreeGamesOneDeleted()
        {
            RunTest(x => x.GetPlayerStatistics());
        }

        public override IEnumerable<IDataTransferObject> ExpectedDtos()
        {
            yield return new GetPlayerStatisticsDto() { DtoId = AnyGuid(), PlayerName = _player1, GamesPlayed = 1, Winnings = 100, PayIn = 20, Profit = 80, ProfitPerGame = 80 };
            yield return new GetPlayerStatisticsDto() { DtoId = AnyGuid(), PlayerName = _player2, GamesPlayed = 2, Winnings = 20, PayIn = 70, Profit = -50, ProfitPerGame = -25 };
            yield return new GetPlayerStatisticsDto() { DtoId = AnyGuid(), PlayerName = _player3, GamesPlayed = 2, Winnings = 60, PayIn = 60, Profit = 0, ProfitPerGame = 0 };
            yield return new GetPlayerStatisticsDto() { DtoId = AnyGuid(), PlayerName = _player4, GamesPlayed = 1, Winnings = 0, PayIn = 30, Profit = -30, ProfitPerGame = -30 };
        }
    }
}
