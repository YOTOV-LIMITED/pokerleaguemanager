using System.Linq;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokerLeagueManager.UI.Wpf.TestFramework
{
    public class EnterGameResultsScreen : BaseScreen
    {
        public EnterGameResultsScreen(ApplicationUnderTest app)
            : base(app)
        {
        }

        public EnterGameResultsScreen EnterGameDate(string gameDate)
        {
            GameDatePicker.DateAsString = gameDate;
            return this;
        }

        public EnterGameResultsScreen EnterPlayerName(string playerName)
        {
            PlayerNameTextBox.Text = playerName;
            return this;
        }

        public EnterGameResultsScreen EnterPlacing(string playerPlacing)
        {
            PlacingTextBox.Text = playerPlacing;
            return this;
        }

        public EnterGameResultsScreen EnterWinnings(string playerWinnings)
        {
            WinningsTextBox.Text = playerWinnings;
            return this;
        }

        public EnterGameResultsScreen EnterPayIn(string payin)
        {
            PayInTextBox.Text = payin;
            return this;
        }

        public EnterGameResultsScreen ClickAddPlayer()
        {
            Mouse.Click(AddPlayerButton);
            return this;
        }

        public EnterGameResultsScreen ClickDeletePlayer()
        {
            Mouse.Click(DeletePlayerButton);
            return this;
        }

        public EnterGameResultsScreen ClickPlayer(string playerName)
        {
            var item = GetPlayerListItem(playerName);
            if (item == null)
            {
                Assert.Fail(string.Format("Couldn't find list item for {0}", playerName));
            }

            Mouse.Click(item);
            return this;
        }

        public ViewGamesListScreen ClickSaveGame()
        {
            Mouse.Click(SaveGameButton);
            return new ViewGamesListScreen(App);
        }

        public ViewGamesListScreen ClickCancel()
        {
            Mouse.Click(CancelButton);
            return new ViewGamesListScreen(App);
        }

        public EnterGameResultsScreen DismissWarningDialog()
        {
            Mouse.Click(ActionFailedOkButton);
            return this;
        }

        public EnterGameResultsScreen VerifyDuplicateGameDateWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("DATE"), "Did not contain DATE: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifyNotEnoughPlayersWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("2 PLAYERS"), "Did not contain 2 PLAYERS: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifyDuplicatePlayerWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("CANNOT ADD THE SAME PLAYER"), "Did not contain CANNOT ADD THE SAME PLAYER: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifySaveGameIsDisabled()
        {
            TakeScreenshot();
            Assert.IsFalse(SaveGameButton.Enabled);
            return this;
        }

        public EnterGameResultsScreen VerifyInvalidWinningsWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("WINNINGS"), "Did not contain WINNINGS: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifyInvalidPlacingWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("PLACING"), "Did not contain PLACING: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifyPayInsDoNotEqualWinningsWarning()
        {
            TakeScreenshot();
            Assert.IsTrue(ActionFailedMessage.DisplayText.ToUpper().Contains("PAY IN"), "Did not contain PAY IN: " + ActionFailedMessage.DisplayText.ToUpper());
            return this;
        }

        public EnterGameResultsScreen VerifyPlayerNotInList(string playerName)
        {
            Assert.IsNull(GetPlayerListItem(playerName), string.Format("Player was found in list when it shouldn't be there: {0}", playerName));
            return this;
        }

        public EnterGameResultsScreen VerifyPlayerList(params string[] expectedPlayers)
        {
            TakeScreenshot();
            var actualPlayers = PlayerListItems;

            foreach (var p in expectedPlayers)
            {
                Assert.IsTrue(actualPlayers.Any(x => x.Name == p), string.Format("{0} not found in list", p));
            }

            return this;
        }

        public override void VerifyScreen()
        {
            TakeScreenshot();
            AddPlayerButton.Find();
        }

        private WpfDatePicker GameDatePicker
        {
            get
            {
                var ctl = new WpfDatePicker(App);
                ctl.SearchProperties.Add(WpfDatePicker.PropertyNames.AutomationId, "GameDatePicker");
                return ctl;
            }
        }

        private WpfEdit PlayerNameTextBox
        {
            get
            {
                var ctl = new WpfEdit(App);
                ctl.SearchProperties.Add(WpfEdit.PropertyNames.AutomationId, "PlayerNameTextBox");
                return ctl;
            }
        }

        private WpfEdit PlacingTextBox
        {
            get
            {
                var ctl = new WpfEdit(App);
                ctl.SearchProperties.Add(WpfEdit.PropertyNames.AutomationId, "PlacingTextBox");
                return ctl;
            }
        }

        private WpfEdit WinningsTextBox
        {
            get
            {
                var ctl = new WpfEdit(App);
                ctl.SearchProperties.Add(WpfEdit.PropertyNames.AutomationId, "WinningsTextBox");
                return ctl;
            }
        }

        private WpfEdit PayInTextBox
        {
            get
            {
                var ctl = new WpfEdit(App);
                ctl.SearchProperties.Add(WpfEdit.PropertyNames.AutomationId, "PayInTextBox");
                return ctl;
            }
        }

        private WpfButton AddPlayerButton
        {
            get
            {
                var ctl = new WpfButton(App);
                ctl.SearchProperties.Add(WpfButton.PropertyNames.AutomationId, "AddPlayerButton");
                return ctl;
            }
        }

        private WpfButton SaveGameButton
        {
            get
            {
                var ctl = new WpfButton(App);
                ctl.SearchProperties.Add(WpfButton.PropertyNames.AutomationId, "SaveGameButton");
                return ctl;
            }
        }

        private WpfButton CancelButton
        {
            get
            {
                var ctl = new WpfButton(App);
                ctl.SearchProperties.Add(WpfButton.PropertyNames.AutomationId, "CancelButton");
                return ctl;
            }
        }

        private WinWindow ActionFailedMessageBox
        {
            get
            {
                var ctl = new WinWindow();
                ctl.SearchProperties.Add(WinWindow.PropertyNames.Name, "Action Failed");
                return ctl;
            }
        }

        private WinButton ActionFailedOkButton
        {
            get
            {
                var ctl = new WinButton(ActionFailedMessageBox);
                ctl.SearchProperties.Add(WinButton.PropertyNames.Name, "OK");
                return ctl;
            }
        }

        private WinText ActionFailedMessage
        {
            get
            {
                var ctl = new WinText(ActionFailedMessageBox);
                return ctl;
            }
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

        private WpfListItem GetPlayerListItem(string playerName)
        {
            return (WpfListItem)PlayerListItems.OfType<WpfListItem>().FirstOrDefault(i => ((WpfListItem)i).DisplayText.Contains(playerName));
        }

        private WpfButton DeletePlayerButton
        {
            get
            {
                var ctl = new WpfButton(App);
                ctl.SearchProperties.Add(WpfButton.PropertyNames.AutomationId, "DeletePlayerButton");
                return ctl;
            }
        }
    }
}
