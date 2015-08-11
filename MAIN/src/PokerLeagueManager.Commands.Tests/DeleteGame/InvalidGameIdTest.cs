using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class InvalidGameIdTest : BaseCommandTest
    {
        [TestMethod]
        public void InvalidGameId()
        {
            RunTest(new DeleteGameCommand() { GameId = Guid.NewGuid() });
        }

        public override Exception ExpectedException()
        {
            return new ArgumentException();
        }
    }
}
