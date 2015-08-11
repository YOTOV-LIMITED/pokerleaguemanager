using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class CreateGameWithErrorThenFixAndResaveTest
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
        public void CreateGameWithErrorThenFixAndResave()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            var enterGameScreen = _gamesListScreen.ClickAddGame();

            enterGameScreen.EnterGameDate(testDate)
                           .EnterPlayerName("Stephen Colbert")
                           .EnterPlacing("1")
                           .EnterWinnings("190")
                           .EnterPayIn("100")
                           .ClickAddPlayer()
                           .ClickSaveGame();

            enterGameScreen.DismissWarningDialog()
                           .EnterPlayerName("Jon Stewart")
                           .EnterPlacing("2")
                           .EnterWinnings("0")
                           .EnterPayIn("90")
                           .ClickAddPlayer()
                           .ClickSaveGame()
                           .VerifyGameInList(testDate + " - Stephen Colbert [$190]");
        }
    }
}
