using System;
using System.Linq;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Common.Commands;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Tests.Infrastructure;
using PokerLeagueManager.UI.Wpf.ViewModels;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf.Tests
{
    [TestClass]
    public class EnterGameResultsViewModelTests
    {
        private ILog _mockLogger = new Mock<ILog>().Object;

        [TestMethod]
        public void WhenGameDateIsEmpty_SaveCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.GameDate = null;

            Assert.IsFalse(sut.SaveGameCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenGameDateIsValid_SaveCommandCanExecuteIsTrue()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.GameDate = DateTime.Now;

            Assert.IsTrue(sut.SaveGameCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlayerDataIsEmpty_AddPlayerCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = string.Empty;
            sut.NewPlacing = string.Empty;
            sut.NewWinnings = string.Empty;
            sut.NewPayIn = string.Empty;

            Assert.IsFalse(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlayerNameIsEmpty_AddPlayerCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = string.Empty;
            sut.NewPlacing = "1";
            sut.NewWinnings = "100";
            sut.NewPayIn = "20";

            Assert.IsFalse(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlacingIsEmpty_AddPlayerCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Homer Simpson";
            sut.NewPlacing = string.Empty;
            sut.NewWinnings = "100";
            sut.NewPayIn = "20";

            Assert.IsFalse(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenWinningsIsEmpty_AddPlayerCommandCanExecuteIsTrue()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Anderson Silva";
            sut.NewPlacing = "5";
            sut.NewWinnings = string.Empty;
            sut.NewPayIn = "20";

            Assert.IsTrue(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlayerDataIsValid_AddPlayerCommandCanExecuteIsTrue()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Keira Knightly";
            sut.NewPlacing = "1";
            sut.NewWinnings = "225";
            sut.NewPayIn = "20";

            Assert.IsTrue(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlacingIsNotANumber_AddPlayerCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Tom Brady";
            sut.NewPlacing = "1st";
            sut.NewWinnings = "150";
            sut.NewPayIn = "20";

            Assert.IsFalse(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenWinningsIsNotANumber_AddPlayerCommandCanExecuteIsFalse()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Tom Brady";
            sut.NewPlacing = "1";
            sut.NewWinnings = "One Hundred Dollars";
            sut.NewPayIn = "20";

            Assert.IsFalse(sut.AddPlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddPlayerWithValidData()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Dylan Smith";
            sut.NewPlacing = "1";
            sut.NewWinnings = "500";
            sut.NewPayIn = "100";

            var watcher = new NotifyPropertyChangedWatcher(sut);

            sut.AddPlayerCommand.Execute(null);

            Assert.AreEqual(string.Empty, sut.NewPlayerName);
            Assert.AreEqual(string.Empty, sut.NewPlacing);
            Assert.AreEqual("0", sut.NewWinnings);

            Assert.AreEqual(1, sut.Players.Count());
            Assert.AreEqual("1 - Dylan Smith [Win: $500] [Pay: $100]", sut.Players.First());

            Assert.IsTrue(watcher.HasPropertyChanged("NewPlayerName"));
            Assert.IsTrue(watcher.HasPropertyChanged("NewPlacing"));
            Assert.IsTrue(watcher.HasPropertyChanged("NewWinnings"));
            Assert.IsTrue(watcher.HasPropertyChanged("Players"));
        }

        [TestMethod]
        public void AddPlayerWithEmptyWinningsShouldConvertToZero()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            sut.NewPlayerName = "Dylan Smith";
            sut.NewPlacing = "1";
            sut.NewWinnings = string.Empty;
            sut.NewPayIn = string.Empty;

            sut.AddPlayerCommand.Execute(null);

            Assert.AreEqual("1 - Dylan Smith [Win: $0] [Pay: $0]", sut.Players.First());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPlayerCommandExecuteIsCalledWhenThereIsInvalidData_ShouldThrowException()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Dylan Smith";
            sut.NewPlacing = string.Empty;
            sut.NewWinnings = "500";

            sut.AddPlayerCommand.Execute(null);
        }

        [TestMethod]
        public void SaveGameWithValidDataNoPlayers()
        {
            var fakeCommandService = new FakeCommandService();
            var mockMainWindow = new Mock<IMainWindow>();
            var mockViewGamesListView = new Mock<IViewGamesListView>();
            Resolver.Container.RegisterInstance<IViewGamesListView>(mockViewGamesListView.Object);

            var sut = new EnterGameResultsViewModel(fakeCommandService, new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            var testGameDate = DateTime.Now;

            sut.GameDate = testGameDate;
            sut.NewPlayerName = "foo";
            sut.NewPlacing = "foo";
            sut.NewWinnings = "foo";
            sut.NewPayIn = "foo";

            sut.SaveGameCommand.Execute(null);

            Assert.AreEqual(1, fakeCommandService.ExecutedCommands.Count);
            EnterGameResultsCommand actualCommand = fakeCommandService.ExecutedCommands[0] as EnterGameResultsCommand;
            Assert.IsFalse(actualCommand.CommandId == Guid.Empty);
            Assert.AreEqual(testGameDate, actualCommand.GameDate);
            Assert.AreEqual(0, actualCommand.Players.Count());

            mockMainWindow.Verify(x => x.ShowView(It.IsAny<IViewGamesListView>()));
        }

        [TestMethod]
        public void SaveGameWithValidDataTwoPlayers()
        {
            var fakeCommandService = new FakeCommandService();
            var mockMainWindow = new Mock<IMainWindow>();
            var mockViewGamesListView = new Mock<IViewGamesListView>();
            Resolver.Container.RegisterInstance<IViewGamesListView>(mockViewGamesListView.Object);

            var sut = new EnterGameResultsViewModel(fakeCommandService, new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            var testGameDate = DateTime.Now;

            sut.GameDate = testGameDate;

            sut.NewPlayerName = "Ryan Fritsch";
            sut.NewPlacing = "1";
            sut.NewWinnings = "200";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            sut.NewPlayerName = "Dylan Smith";
            sut.NewPlacing = "2";
            sut.NewWinnings = "0";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            sut.SaveGameCommand.Execute(null);

            Assert.AreEqual(1, fakeCommandService.ExecutedCommands.Count);

            EnterGameResultsCommand actualCommand = fakeCommandService.ExecutedCommands[0] as EnterGameResultsCommand;

            Assert.IsFalse(actualCommand.CommandId == Guid.Empty);
            Assert.AreEqual(testGameDate, actualCommand.GameDate);
            Assert.AreEqual(2, actualCommand.Players.Count());

            var ryanPlayer = actualCommand.Players.First(x => x.PlayerName == "Ryan Fritsch");
            var dylanPlayer = actualCommand.Players.First(x => x.PlayerName == "Dylan Smith");

            Assert.AreEqual(1, ryanPlayer.Placing);
            Assert.AreEqual(200, ryanPlayer.Winnings);
            Assert.AreEqual(100, ryanPlayer.PayIn);

            Assert.AreEqual(2, dylanPlayer.Placing);
            Assert.AreEqual(0, dylanPlayer.Winnings);
            Assert.AreEqual(100, dylanPlayer.PayIn);

            mockMainWindow.Verify(x => x.ShowView(It.IsAny<IViewGamesListView>()));
        }

        [TestMethod]
        public void AddTwoPlayersShouldReturnProperPlayerStringsInProperOrder()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.NewPlayerName = "Dylan Smith";
            sut.NewPlacing = "2";
            sut.NewWinnings = "0";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            sut.NewPlayerName = "Ryan Fritsch";
            sut.NewPlacing = "1";
            sut.NewWinnings = "200";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            Assert.AreEqual(2, sut.Players.Count());
            Assert.AreEqual("1 - Ryan Fritsch [Win: $200] [Pay: $100]", sut.Players.ElementAt(0));
            Assert.AreEqual("2 - Dylan Smith [Win: $0] [Pay: $100]", sut.Players.ElementAt(1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SaveGameWithNoGameDate()
        {
            var mockMainWindow = new Mock<IMainWindow>();
            var sut = new EnterGameResultsViewModel(new FakeCommandService(), new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.GameDate = null;

            sut.SaveGameCommand.Execute(null);
        }

        [TestMethod]
        public void ClickCancelButtonShouldShowViewGamesList()
        {
            var mockMainWindow = new Mock<IMainWindow>();

            var mockView = new Mock<IViewGamesListView>();
            Resolver.Container.RegisterInstance<IViewGamesListView>(mockView.Object);

            var sut = new EnterGameResultsViewModel(null, new Mock<IQueryService>().Object, mockMainWindow.Object, _mockLogger);

            sut.CancelCommand.Execute(null);

            mockMainWindow.Verify(x => x.ShowView(mockView.Object));
        }

        [TestMethod]
        public void WhenPlayerCountIsZero_CanDeleteIsFalse()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            Assert.IsFalse(sut.DeletePlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenNoPlayerIsSelected_CanDeleteIsFalse()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            sut.NewPlayerName = "Dylan";
            sut.NewPlacing = "1";
            sut.NewWinnings = "100";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            sut.SelectedPlayerIndex = -1;

            Assert.IsFalse(sut.DeletePlayerCommand.CanExecute(null));
        }

        [TestMethod]
        public void WhenPlayerIsSelected_CanDeleteIsTrue()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            sut.NewPlayerName = "Dylan";
            sut.NewPlacing = "1";
            sut.NewWinnings = "100";
            sut.NewPayIn = "100";
            sut.AddPlayerCommand.Execute(null);

            sut.SelectedPlayerIndex = 0;

            Assert.IsTrue(sut.DeletePlayerCommand.CanExecute(null));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletePlayerCalledWhenNotValid()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            sut.DeletePlayerCommand.Execute(null);
        }

        [TestMethod]
        public void DeletePlayerRemovesFromList()
        {
            var sut = new EnterGameResultsViewModel(null, null, null, null);

            sut.NewPlayerName = "Dylan";
            sut.NewPlacing = "1";
            sut.NewWinnings = "100";
            sut.NewPayIn = "50";
            sut.AddPlayerCommand.Execute(null);

            sut.NewPlayerName = "Super Mario";
            sut.NewPlacing = "2";
            sut.NewWinnings = "50";
            sut.NewPayIn = "50";
            sut.AddPlayerCommand.Execute(null);

            sut.NewPlayerName = "Yoshi";
            sut.NewPlacing = "3";
            sut.NewWinnings = "0";
            sut.NewPayIn = "50";
            sut.AddPlayerCommand.Execute(null);

            sut.SelectedPlayerIndex = 1;

            sut.DeletePlayerCommand.Execute(null);

            Assert.AreEqual(2, sut.Players.Count());
            Assert.AreEqual("1 - Dylan [Win: $100] [Pay: $50]", sut.Players.ElementAt(0));
            Assert.AreEqual("3 - Yoshi [Win: $0] [Pay: $50]", sut.Players.ElementAt(1));
        }
    }
}
