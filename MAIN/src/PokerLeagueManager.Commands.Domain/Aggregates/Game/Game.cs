using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Events;

namespace PokerLeagueManager.Commands.Domain.Aggregates
{
    public class Game : BaseAggregateRoot
    {
        private List<Player> _players = new List<Player>();

        public Game(Guid gameId, DateTime gameDate)
        {
            if (gameDate == DateTime.MinValue || gameDate == DateTime.MaxValue || gameDate == default(DateTime))
            {
                throw new InvalidGameDateException(gameDate);
            }

            gameId = gameId == Guid.Empty ? Guid.NewGuid() : gameId;

            this.PublishEvent(new GameCreatedEvent() { AggregateId = gameId, GameDate = gameDate });
        }

        private Game()
        {
        }

        public void AddPlayer(string playerName, int placing, int winnings, int payin)
        {
            if (winnings < 0)
            {
                throw new WinningsCannotBeNegativeException(winnings, playerName);
            }

            if (placing <= 0)
            {
                throw new PlacingMustBeGreaterThanZeroException(placing, playerName);
            }

            if (payin <= 0)
            {
                throw new PayinMustBeGreaterThanZeroException(payin, playerName);
            }

            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new PlayerNameMustNotBeBlankException();
            }

            if (_players.Any(x => x.PlayerName.ToUpper().Trim() == playerName.ToUpper().Trim()))
            {
                throw new DuplicatePlayerNameException(playerName);
            }

            this.PublishEvent(new PlayerAddedToGameEvent() { AggregateId = AggregateId, PlayerName = playerName, Placing = placing, Winnings = winnings, PayIn = payin });
        }

        public void ValidateGame()
        {
            if (_players.Count < 2)
            {
                throw new GameWithNotEnoughPlayersException();
            }

            var orderedPlayers = _players.OrderBy(x => x.Placing);

            var curPlacing = 1;

            foreach (var curPlayer in orderedPlayers)
            {
                if (curPlayer.Placing != curPlacing++)
                {
                    throw new PlayerPlacingsNotInOrderException();
                }
            }

            if (_players.Sum(p => p.Winnings) != _players.Sum(p => p.Payin))
            {
                throw new WinningsDoesNotEqualPayInsException();
            }
        }

        public void DeleteGame()
        {
            base.PublishEvent(new GameDeletedEvent() { AggregateId = base.AggregateId });
        }

        public void RenamePlayer(string oldPlayerName, string newPlayerName)
        {
            var duplicatePlayer = _players.FirstOrDefault(p => p.PlayerName == newPlayerName);

            if (duplicatePlayer != null)
            {
                throw new DuplicatePlayerNameException(newPlayerName);
            }

            var oldPlayer = _players.FirstOrDefault(p => p.PlayerName == oldPlayerName);

            if (oldPlayer != null)
            {
                base.PublishEvent(new PlayerRenamedEvent() { AggregateId = base.AggregateId, OldPlayerName = oldPlayerName, NewPlayerName = newPlayerName });
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Is called via reflection")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Plumbing needs this method signature to exist to work properly")]
        private void ApplyEvent(GameCreatedEvent e)
        {
            AggregateId = e.AggregateId;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Is called via reflection")]
        private void ApplyEvent(PlayerAddedToGameEvent e)
        {
            _players.Add(new Player() { PlayerName = e.PlayerName, Placing = e.Placing, Winnings = e.Winnings, Payin = e.PayIn });
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Is called via reflection")]
        private void ApplyEvent(PlayerRenamedEvent e)
        {
            _players.Single(p => p.PlayerName == e.OldPlayerName).PlayerName = e.NewPlayerName;
        }
    }
}
