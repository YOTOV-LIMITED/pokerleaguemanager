using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Tests.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Tests.EnterGameResults
{
    [TestClass]
    public class DuplicatePlayersCaseInsensitiveTest : BaseCommandTest
    {
        private DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        [TestMethod]
        public void DuplicatePlayersCaseInsensitive()
        {
            var players = new List<EnterGameResultsCommand.GamePlayer>();
            players.Add(new EnterGameResultsCommand.GamePlayer() { PlayerName = "Dylan Smith", Placing = 1, Winnings = 100, PayIn = 50 });
            players.Add(new EnterGameResultsCommand.GamePlayer() { PlayerName = "dylan smith", Placing = 2, Winnings = 0, PayIn = 50 });

            RunTest(new EnterGameResultsCommand() { GameDate = _gameDate, Players = players });
        }

        public override Exception ExpectedException()
        {
            return new DuplicatePlayerNameException("Dylan Smith");
        }
    }
}
