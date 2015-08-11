using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Common.Events.Infrastructure
{
    public static class Bootstrapper
    {
        private static bool _hasBootstrapped = false;

        public static void Bootstrap()
        {
            if (!_hasBootstrapped)
            {
                PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();

                _hasBootstrapped = true;
            }
        }
    }
}