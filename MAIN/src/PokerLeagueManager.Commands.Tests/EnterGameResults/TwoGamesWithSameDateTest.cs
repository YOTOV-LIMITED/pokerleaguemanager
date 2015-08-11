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
    public class TwoGamesWithSameDateTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = Guid.NewGuid(), GameDate = _gameDate };
        }

        [TestMethod]
        public void TwoGamesWithSameDate()
        {
            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate });
        }

        public override Exception ExpectedException()
        {
            return new ArgumentException();
        }
    }
}
