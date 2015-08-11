using System.Linq;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokerLeagueManager.UI.Wpf.TestFramework
{
    public class ViewGameResultsScreen : BaseScreen
    {
        public ViewGameResultsScreen(ApplicationUnderTest app)
            : base(app)
        {
        }

        public ViewGamesListScreen ClickClose()
        {
            Mouse.Click(CloseButton);
            return new ViewGamesListScreen(App);
        }

        public ViewGameResultsScreen VerifyGameDate(string gameDate)
        {
            Assert.AreEqual(gameDate, GameDateTextBox.Text);
            return this;
        }

        public override void VerifyScreen()
        {
            TakeScreenshot();
            CloseButton.Find();
        }

        private WpfButton CloseButton
        {
            get
            {
                var ctl = new WpfButton(App);
                ctl.SearchProperties.Add(WpfButton.PropertyNames.AutomationId, "CloseButton");
                return ctl;
            }
        }

        public ViewGameResultsScreen VerifyPlayerList(params string[] expectedPlayers)
        {
            var actualPlayers = PlayerListItems;

            foreach (var p in expectedPlayers)
            {
                Assert.IsTrue(actualPlayers.Any(x => x.Name == p), string.Format("{0} not found in list", p));
            }

            return this;
        }

        private UITestControlCollection PlayerListItems
        {
            get
            {
                var list = new WpfList(App);
                list.SearchProperties.Add(WpfList.PropertyNames.AutomationId, "PlayersListBox");

                var items = new WpfListItem(list);
                return items.FindMatchingControls();
            }
        }

        private WpfEdit GameDateTextBox
        {
            get
            {
                var ctl = new WpfEdit(App);
                ctl.SearchProperties.Add(WpfEdit.PropertyNames.AutomationId, "GameDateTextBox");
                return ctl;
            }
        }
    }
}
