using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class NegativeWinningsTest
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
        public void NegativeWinnings()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            var enterGameScreen = _gamesListScreen.ClickAddGame();

            enterGameScreen.EnterGameDate(testDate)
                           .EnterPlayerName("Jerry Seinfeld")
                           .EnterPlacing("1")
                           .EnterWinnings("130")
                           .EnterPayIn("55")
                           .ClickAddPlayer()
                           .EnterPlayerName("Wayne Gretzky")
                           .EnterPlacing("2")
                           .EnterWinnings("-20")
                           .EnterPayIn("55")
                           .ClickAddPlayer()
                           .ClickSaveGame();

            enterGameScreen.VerifyInvalidWinningsWarning();

            enterGameScreen.DismissWarningDialog()
                           .VerifyScreen();
        }
    }
}
