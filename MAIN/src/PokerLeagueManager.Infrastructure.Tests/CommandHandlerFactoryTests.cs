using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands;
using PokerLeagueManager.Common.DTO;

namespace PokerLeagueManager.Infrastructure.Tests
{
    [TestClass]
    public class CommandHandlerFactoryTests
    {
        [TestMethod]
        public void ShouldLogCommand()
        {
            var testCommand = new EnterGameResultsCommand();
            testCommand.GameDate = DateTime.Now.AddDays(-2);
            testCommand.CommandId = Guid.NewGuid();
            testCommand.GameId = Guid.NewGuid();
            testCommand.IPAddress = "12.34.56.78";
            testCommand.User = "dylans";
            testCommand.Timestamp = DateTime.Now;

            var newPlayerA = new EnterGameResultsCommand.GamePlayer();
            newPlayerA.PlayerName = "Dylan Smith";
            newPlayerA.Placing = 1;
            newPlayerA.Winnings = 120;
            newPlayerA.PayIn = 60;

            var newPlayerB = new EnterGameResultsCommand.GamePlayer();
            newPlayerB.PlayerName = "Homer Simpson";
            newPlayerB.Placing = 2;
            newPlayerB.Winnings = 0;
            newPlayerB.PayIn = 60;

            var players = new List<EnterGameResultsCommand.GamePlayer>();
            players.Add(newPlayerA);
            players.Add(newPlayerB);

            testCommand.Players = players;

            var mockCommandRepository = new Mock<ICommandRepository>();

            var sut = new CommandHandlerFactory(
                new Mock<IEventRepository>().Object,
                new Mock<IQueryService>().Object,
                mockCommandRepository.Object);

            sut.ExecuteCommand(testCommand);

            mockCommandRepository.Verify(x => x.LogCommand(testCommand));
        }

        [TestMethod]
        public void ShouldLogFailedCommand()
        {
            var testCommand = new EnterGameResultsCommand();
            testCommand.GameDate = DateTime.Now.AddDays(-2);
            testCommand.CommandId = Guid.NewGuid();
            testCommand.GameId = Guid.NewGuid();
            testCommand.IPAddress = "12.34.56.78";
            testCommand.User = "dylans";
            testCommand.Timestamp = DateTime.Now;

            var newPlayerA = new EnterGameResultsCommand.GamePlayer();
            newPlayerA.PlayerName = "Dylan Smith";
            newPlayerA.Placing = 1;
            newPlayerA.Winnings = 120;
            newPlayerA.PayIn = 60;

            var newPlayerB = new EnterGameResultsCommand.GamePlayer();
            newPlayerB.PlayerName = "Homer Simpson";
            newPlayerB.Placing = 2;
            newPlayerB.Winnings = 0;
            newPlayerB.PayIn = 60;

            var players = new List<EnterGameResultsCommand.GamePlayer>();
            players.Add(newPlayerA);
            players.Add(newPlayerB);

            testCommand.Players = players;

            var mockCommandRepository = new Mock<ICommandRepository>();
            var mockEventRepository = new Mock<IEventRepository>();
            var mockQueryService = new Mock<IQueryService>();

            var sut = new CommandHandlerFactory(
                mockEventRepository.Object,
                mockQueryService.Object,
                mockCommandRepository.Object);

            var ex = new ArgumentException("foo");
            mockEventRepository.Setup(x => x.PublishEvents(It.IsAny<IAggregateRoot>(), testCommand)).Throws(ex);

            try
            {
                sut.ExecuteCommand(testCommand);
            }
            catch
            {
                // eat all exceptions
            }

            mockCommandRepository.Verify(x => x.LogCommand(testCommand));
            mockCommandRepository.Verify(x => x.LogFailedCommand(testCommand, ex));
        }
    }
}
