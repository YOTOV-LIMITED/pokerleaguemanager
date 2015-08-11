using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests
{
    [TestClass]
    public class TwoGamesTest : BaseQueryTest
    {
        private Guid _gameId1 = Guid.NewGuid();
        private Guid _gameId2 = Guid.NewGuid();
        
        private DateTime _gameDate1 = DateTime.Parse("17-Feb-2014");
        private DateTime _gameDate2 = DateTime.Parse("24-Feb-2014");

        private string _player1_1 = "Dylan";
        private string _player2_1 = "Ryan";
        private string _player1_2 = "Chris";
        private string _player2_2 = "Colin";

        private int _winnings1_1 = 123;
        private int _winnings2_1 = 10;
        private int _winnings1_2 = 45;
        private int _winnings2_2 = 0;

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId1, GameDate = _gameDate1 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player1_1, Placing = 1, Winnings = _winnings1_1 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player2_1, Placing = 2, Winnings = _winnings2_1 };

            yield return new GameCreatedEvent() { AggregateId = _gameId2, GameDate = _gameDate2 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player2_2, Placing = 2, Winnings = _winnings2_2 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player1_2, Placing = 1, Winnings = _winnings1_2 };
        }

        [TestMethod]
        public void GetGamesList_TwoGames()
        {
            RunTest(x => x.GetGamesList());
        }

        public override IEnumerable<IDataTransferObject> ExpectedDtos()
        {
            yield return new GetGamesListDto() { DtoId = AnyGuid(), GameId = _gameId1, GameDate = _gameDate1, Winner = _player1_1, Winnings = _winnings1_1 };
            yield return new GetGamesListDto() { DtoId = AnyGuid(), GameId = _gameId2, GameDate = _gameDate2, Winner = _player1_2, Winnings = _winnings1_2 };
        }
    }
}
