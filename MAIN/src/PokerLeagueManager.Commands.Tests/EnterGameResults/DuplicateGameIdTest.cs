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
    public class DuplicateGameIdTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");
        private Guid _gameId = Guid.NewGuid();

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId, GameDate = DateTime.Parse("02-Jul-2007") };
        }

        [TestMethod]
        public void DuplicateGameId()
        {
            RunTest(new EnterGameResultsCommand() { GameId = _gameId, GameDate = _gameDate });
        }

        public override Exception ExpectedException()
        {
            return new ArgumentException();
        }
    }
}
