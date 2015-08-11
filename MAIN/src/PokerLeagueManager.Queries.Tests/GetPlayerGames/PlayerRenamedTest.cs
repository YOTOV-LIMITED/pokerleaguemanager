using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests.GetPlayerGames
{
    [TestClass]
    public class PlayerRenamedTest : BaseQueryTest
    {
        private Guid _gameId1 = Guid.NewGuid();
        private Guid _gameId2 = Guid.NewGuid();
        private Guid _gameId3 = Guid.NewGuid();

        private DateTime _gameDate1 = DateTime.Now;
        private DateTime _gameDate2 = DateTime.Now;
        private DateTime _gameDate3 = DateTime.Now;

        private string _player1 = "Luke Skywalker";
        private string _player2 = "Yoda";
        private string _player3 = "Chewbacca";

        private string _newPlayerName = "Darth Vader";

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId1, GameDate = _gameDate1 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId1, PlayerName = _player3, Placing = 3, Winnings = 0, PayIn = 20 };

            yield return new GameCreatedEvent() { AggregateId = _gameId2, GameDate = _gameDate2 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player1, Placing = 2, Winnings = 10, PayIn = 20 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId2, PlayerName = _player3, Placing = 3, Winnings = 0, PayIn = 20 };

            yield return new GameCreatedEvent() { AggregateId = _gameId3, GameDate = _gameDate3 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId3, PlayerName = _player3, Placing = 1, Winnings = 60, PayIn = 40 };
            yield return new PlayerAddedToGameEvent { AggregateId = _gameId3, PlayerName = _player2, Placing = 2, Winnings = 0, PayIn = 20 };

            yield return new PlayerRenamedEvent() { AggregateId = _gameId1, OldPlayerName = _player3, NewPlayerName = _newPlayerName };
        }

        [TestMethod]
        public void GetPlayerGames_PlayerRenamedTest()
        {
            RunTest(x => x.GetPlayerGames(_newPlayerName));
        }

        public override IEnumerable<IDataTransferObject> ExpectedDtos()
        {
            yield return new GetPlayerGamesDto() { GameId = _gameId1, GameDate = _gameDate1, PlayerName = _newPlayerName, Placing = 3, Winnings = 0, PayIn = 20 };
            yield return new GetPlayerGamesDto() { GameId = _gameId2, GameDate = _gameDate2, PlayerName = _newPlayerName, Placing = 3, Winnings = 0, PayIn = 20 };
            yield return new GetPlayerGamesDto() { GameId = _gameId3, GameDate = _gameDate3, PlayerName = _newPlayerName, Placing = 1, Winnings = 60, PayIn = 40 };
        }
    }
}
