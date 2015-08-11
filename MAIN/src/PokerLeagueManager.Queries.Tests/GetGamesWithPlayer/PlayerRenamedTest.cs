using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests.GetGamesWithPlayer
{
    [TestClass]
    public class PlayerRenamedTest : BaseQueryTest
    {
        private Guid _gameId1 = Guid.NewGuid();
        private Guid _gameId2 = Guid.NewGuid();
        private Guid _gameId3 = Guid.NewGuid();

        private string _player1 = "Dylan";
        private string _player2 = "Ryan";
        private string _player3 = "Chris";
        private string _player4 = "Jeff";

        private string _newPlayerName = "Norm MacDonald";

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

            yield return new PlayerRenamedEvent() { AggregateId = _gameId3, OldPlayerName = _player2, NewPlayerName = _newPlayerName };
        }

        [TestMethod]
        public void GetGamesWithPlayer_PlayerRenamedTest()
        {
            RunTest(x => x.GetGamesWithPlayer(_newPlayerName));
        }

        public override IEnumerable<IDataTransferObject> ExpectedDtos()
        {
            yield return new GetGamesWithPlayerDto() { GameId = _gameId1, PlayerName = _newPlayerName };
            yield return new GetGamesWithPlayerDto() { GameId = _gameId3, PlayerName = _newPlayerName };
        }
    }
}
