using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class DuplicatePlayerNamesTest
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
        public void DuplicatePlayerNames()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            var enterGameScreen = _gamesListScreen.ClickAddGame();

            enterGameScreen.EnterGameDate(testDate)
                           .EnterPlayerName("Jennifer Aniston")
                           .EnterPlacing("1")
                           .EnterWinnings("130")
                           .EnterPayIn("75")
                           .ClickAddPlayer()
                           .EnterPlayerName("Jennifer Aniston")
                           .EnterPlacing("2")
                           .EnterWinnings("20")
                           .EnterPayIn("75")
                           .ClickAddPlayer()
                           .ClickSaveGame();

            enterGameScreen.VerifyDuplicatePlayerWarning();

            enterGameScreen.DismissWarningDialog()
                           .VerifyScreen();
        }
    }
}
