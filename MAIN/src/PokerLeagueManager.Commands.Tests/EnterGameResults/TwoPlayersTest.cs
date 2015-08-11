using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class TwoPlayersTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        [TestMethod]
        public void TwoPlayers()
        {
            var players = new List<EnterGameResultsCommand.GamePlayer>();
            players.Add(new EnterGameResultsCommand.GamePlayer() { PlayerName = "Dylan Smith", Placing = 1, Winnings = 100, PayIn = 30 });
            players.Add(new EnterGameResultsCommand.GamePlayer() { PlayerName = "Grant Hirose", Placing = 2, Winnings = 50, PayIn = 120 });

            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate, Players = players });
        }

        public override IEnumerable<IEvent> ExpectedEvents()
        {
            yield return new GameCreatedEvent() { AggregateId = AnyGuid(), GameDate = _gameDate };
            yield return new VerifyEventsNow();

            yield return new PlayerAddedToGameEvent() { AggregateId = AnyGuid(), PlayerName = "Grant Hirose", Placing = 2, Winnings = 50, PayIn = 120 };
            yield return new PlayerAddedToGameEvent() { AggregateId = AnyGuid(), PlayerName = "Dylan Smith", Placing = 1, Winnings = 100, PayIn = 30 };
        }
    }
}
