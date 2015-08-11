using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class DeletePlayerFromGameTest
    {
        private ApplicationUnderTest _app;
        private ViewGamesListScreen _gamesListScreen;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = ApplicationUnderTest.Launch(@"C:\PokerLeagueManager.UI.Wpf\PokerLeagueManager.UI.Wpf.exe");
            _gamesListScreen = new ViewGamesListScreen(_app);
        }

        [TestMethod]
        public void DeletePlayerFromGame()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            var enterGameScreen = _gamesListScreen.ClickAddGame();

            enterGameScreen.EnterGameDate(testDate)
                           .EnterPlayerName("Jerry Seinfeld")
                           .EnterPlacing("1")
                           .EnterWinnings("130")
                           .EnterPayIn("130")
                           .ClickAddPlayer()
                           .EnterPlayerName("Wayne Gretzky")
                           .EnterPlacing("2")
                           .EnterWinnings("20")
                           .EnterPayIn("20")
                           .ClickAddPlayer()
                           .ClickPlayer("Wayne Gretzky")
                           .ClickDeletePlayer()
                           .ClickSaveGame();

            enterGameScreen.VerifyNotEnoughPlayersWarning()
                           .DismissWarningDialog()
                           .VerifyPlayerNotInList("Wayne Gretzky");
        }
    }
}
