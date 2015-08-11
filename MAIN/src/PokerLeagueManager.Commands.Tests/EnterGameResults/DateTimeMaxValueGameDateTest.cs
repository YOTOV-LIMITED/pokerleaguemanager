using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class DateTimeMaxValueGameDateTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.MaxValue;

        [TestMethod]
        public void DateTimeMaxValueGameDate()
        {
            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate });
        }

        public override Exception ExpectedException()
        {
            return new InvalidGameDateException(_gameDate);
        }
    }
}
