using Microsoft.VisualStudio.TestTools.UITesting;

namespace PokerLeagueManager.UI.Wpf.TestFramework
{
    public class BaseScreen
    {
        public BaseScreen(ApplicationUnderTest app)
        {
            App = app;
        }

        public void TakeScreenshot()
        {
            Keyboard.SendKeys("{DOWN}");
        }

        public virtual void VerifyScreen()
        {
            TakeScreenshot();
        }

        protected ApplicationUnderTest App { get; set; }
    }
}
