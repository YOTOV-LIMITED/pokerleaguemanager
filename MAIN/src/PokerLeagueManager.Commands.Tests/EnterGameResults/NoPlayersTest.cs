using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class NoPlayersTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        [TestMethod]
        public void NoPlayers()
        {
            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate });
        }

        public override Exception ExpectedException()
        {
            return new GameWithNotEnoughPlayersException();
        }
    }
}
