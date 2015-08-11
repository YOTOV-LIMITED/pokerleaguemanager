using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class CreateGameWithFourPlayersTest
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
        public void CreateGameWithFourPlayers()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            _gamesListScreen.ClickAddGame()
                            .EnterGameDate(testDate)
                            .EnterPlayerName("Ronda Rousey")
                            .EnterPlacing("2")
                            .EnterWinnings("40")
                            .EnterPayIn("100")
                            .ClickAddPlayer()
                            .EnterPlayerName("Mike Tyson")
                            .EnterPlacing("3")
                            .EnterWinnings("0")
                            .EnterPayIn("100")
                            .ClickAddPlayer()
                            .EnterPlayerName("Georges St Pierre")
                            .EnterPlacing("1")
                            .EnterWinnings("210")
                            .EnterPayIn("25")
                            .ClickAddPlayer()
                            .EnterPlayerName("Peter Griffin")
                            .EnterPlacing("4")
                            .EnterWinnings(string.Empty)
                            .EnterPayIn("25")
                            .ClickAddPlayer()
                            .ClickSaveGame()
                            .VerifyGameInList(testDate + " - Georges St Pierre [$210]");
        }
    }
}
