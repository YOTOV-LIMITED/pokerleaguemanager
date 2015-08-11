using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class NegativeWinningsTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        [TestMethod]
        public void NegativeWinnings()
        {
            var players = new List<EnterGameResultsCommand.GamePlayer>();
            players.Add(new EnterGameResultsCommand.GamePlayer() { PlayerName = "Dylan Smith", Placing = 1, Winnings = -100, PayIn = 50 });

            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate, Players = players });
        }

        public override Exception ExpectedException()
        {
            return new WinningsCannotBeNegativeException(-100, "Dylan Smith");
        }
    }
}
