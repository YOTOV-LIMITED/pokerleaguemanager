using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests.GetGameCountByDate
{
    [TestClass]
    public class TwoGamesThenDeleteOneTest : BaseQueryTest
    {
        private Guid _gameId1 = Guid.NewGuid();
        private Guid _gameId2 = Guid.NewGuid();

        private DateTime _gameDate1 = DateTime.Parse("03-Jul-1981");
        private DateTime _gameDate2 = DateTime.Parse("03-Jul-2015");

        public override IEnumerable<IEvent> Given()
        {
            yield return new GameCreatedEvent() { AggregateId = _gameId1, GameDate = _gameDate1 };
            yield return new GameCreatedEvent() { AggregateId = _gameId2, GameDate = _gameDate2 };
            yield return new GameDeletedEvent() { AggregateId = _gameId1 };
        }

        [TestMethod]
        public void GetGameCountByDate_TwoGamesThenDeleteOne()
        {
            var result1 = SetupQueryService().GetGameCountByDate(_gameDate1);
            var result2 = SetupQueryService().GetGameCountByDate(_gameDate2);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(1, result2);
        }
    }
}
