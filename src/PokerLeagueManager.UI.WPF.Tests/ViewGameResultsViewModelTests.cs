using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Tests.Infrastructure;
using PokerLeagueManager.UI.Wpf.ViewModels;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf.Tests
{
    [TestClass]
    public class ViewGameResultsViewModelTests
    {
        [TestMethod]
        public void WhenGameIdIsSet_GameDateIsSet()
        {
            var gameId = Guid.NewGuid();
            var testResultsDto = new GetGameResultsDto();
            testResultsDto.GameDate = DateTime.Parse("1-Jan-2015");

            var mockQuerySvc = new Mock<IQueryService>();
            mockQuerySvc.Setup(x => x.GetGameResults(gameId)).Returns(testResultsDto);

            var sut = new ViewGameResultsViewModel(null, mockQuerySvc.Object, null, null);

            sut.GameId = gameId;

            Assert.AreEqual("1-Jan-2015", sut.GameDate);
        }

        [TestMethod]
        public void WhenGameIdIsSet_PlayersIsUpdated()
        {
            var gameId = Guid.NewGuid();
            var testResultsDto = new GetGameResultsDto();
            testResultsDto.GameDate = DateTime.Parse("1-Jan-2015");

            var player = new GetGameResultsDto.PlayerDto();
            player.Placing = 1;
            player.PlayerName = "King Kong";
            player.Winnings = 100;
            player.PayIn = 20;
            testResultsDto.Players.Add(player);

            var mockQuerySvc = new Mock<IQueryService>();
            mockQuerySvc.Setup(x => x.GetGameResults(gameId)).Returns(testResultsDto);

            var sut = new ViewGameResultsViewModel(null, mockQuerySvc.Object, null, null);

            sut.GameId = gameId;

            Assert.AreEqual(1, sut.Players.Count());
            Assert.AreEqual("1 - King Kong [Win: $100] [Pay: $20]", sut.Players.First());
        }

        [TestMethod]
        public void WhenGameIdIsSet_NotifyPropertyChangedShouldFire()
        {
            var gameId = Guid.NewGuid();
            var testResultsDto = new GetGameResultsDto();
            testResultsDto.GameDate = DateTime.Parse("1-Jan-2015");

            var player = new GetGameResultsDto.PlayerDto();
            player.Placing = 1;
            player.PlayerName = "King Kong";
            player.Winnings = 100;
            testResultsDto.Players.Add(player);

            var mockQuerySvc = new Mock<IQueryService>();
            mockQuerySvc.Setup(x => x.GetGameResults(gameId)).Returns(testResultsDto);

            var sut = new ViewGameResultsViewModel(null, mockQuerySvc.Object, null, null);

            var watcher = new NotifyPropertyChangedWatcher(sut);

            sut.GameId = gameId;

            Assert.IsTrue(watcher.HasPropertyChanged("GameDate"));
            Assert.IsTrue(watcher.HasPropertyChanged("Players"));
        }

        [TestMethod]
        public void PlayersAreShownInOrderByPlacing()
        {
            var gameId = Guid.NewGuid();
            var testResultsDto = new GetGameResultsDto();
            testResultsDto.GameDate = DateTime.Parse("1-Jan-2015");

            var player1 = new GetGameResultsDto.PlayerDto() { Placing = 3, PlayerName = "King Kong", Winnings = 0, PayIn = 40 };
            var player2 = new GetGameResultsDto.PlayerDto() { Placing = 1, PlayerName = "Donkey Kong", Winnings = 100, PayIn = 40 };
            var player3 = new GetGameResultsDto.PlayerDto() { Placing = 2, PlayerName = "Diddy Kong", Winnings = 0, PayIn = 20 };

            testResultsDto.Players.Add(player1);
            testResultsDto.Players.Add(player2);
            testResultsDto.Players.Add(player3);

            var mockQuerySvc = new Mock<IQueryService>();
            mockQuerySvc.Setup(x => x.GetGameResults(gameId)).Returns(testResultsDto);

            var sut = new ViewGameResultsViewModel(null, mockQuerySvc.Object, null, null);

            sut.GameId = gameId;

            Assert.AreEqual(3, sut.Players.Count());
            Assert.AreEqual("1 - Donkey Kong [Win: $100] [Pay: $40]", sut.Players.ElementAt(0));
            Assert.AreEqual("2 - Diddy Kong [Win: $0] [Pay: $20]", sut.Players.ElementAt(1));
            Assert.AreEqual("3 - King Kong [Win: $0] [Pay: $40]", sut.Players.ElementAt(2));
        }

        [TestMethod]
        public void ClickCloseButtonShouldShowViewGamesList()
        {
            var mockMainWindow = new Mock<IMainWindow>();

            var mockView = new Mock<IViewGamesListView>();
            Resolver.Container.RegisterInstance<IViewGamesListView>(mockView.Object);

            var sut = new ViewGameResultsViewModel(null, null, mockMainWindow.Object, null);

            sut.CloseCommand.Execute(null);

            mockMainWindow.Verify(x => x.ShowView(mockView.Object));
        }
    }
}
