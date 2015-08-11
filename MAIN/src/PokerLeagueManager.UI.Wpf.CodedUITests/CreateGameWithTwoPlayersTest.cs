using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class CreateGameWithTwoPlayersTest
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
        public void CreateGameWithTwoPlayers()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            _gamesListScreen.ClickAddGame()
                            .EnterGameDate(testDate)
                            .EnterPlayerName("Jerry Seinfeld")
                            .EnterPlacing("1")
                            .EnterWinnings("130")
                            .EnterPayIn("75")
                            .ClickAddPlayer()
                            .EnterPlayerName("Wayne Gretzky")
                            .EnterPlacing("2")
                            .EnterWinnings("20")
                            .EnterPayIn("75")
                            .ClickAddPlayer()
                            .ClickSaveGame()
                            .VerifyGameInList(testDate + " - Jerry Seinfeld [$130]");
        }
    }
}
