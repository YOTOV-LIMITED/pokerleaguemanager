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
    public class DeleteGameHappyPathTest : BaseCommandTest
    {
        private Guid _gameId = Guid.NewGuid();

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId, GameDate = DateTime.Now };
        }

        [TestMethod]
        public void DeleteGameHappyPath()
        {
            RunTest(new DeleteGameCommand() { GameId = _gameId });
        }

        public override IEnumerable<IEvent> ExpectedEvents()
        {
            yield return new GameDeletedEvent() { AggregateId = _gameId };
        }
    }
}
