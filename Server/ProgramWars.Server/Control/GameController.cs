using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using ProgramWars.Server.Game;
using ProgramWars.Server.Game.Factories;
using ProgramWars.Server.Models;

namespace ProgramWars.Server.Control
{
    public interface IGameController
    {
        Guid GameId { get; }
        void StartGame();
    }

    public class GameController : IGameController
    {
        private readonly Dictionary<Guid, PlayerPipeline> _playerPipelines;

        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        private readonly IGame _game;

        public Guid GameId { get; }

        public GameController(IGame game, PlayerPipeline player1Pipeline, PlayerPipeline player2Pipeline)
        {
            GameId = Guid.NewGuid();
            _game = game;

            _subscriptions.Add(
                player1Pipeline.Actions.Subscribe(action => ReceiveAction(_game.Player1.PlayerId, action))
                );

            _subscriptions.Add(
                player2Pipeline.Actions.Subscribe(action => ReceiveAction(_game.Player2.PlayerId, action))
                );
            
            _playerPipelines = new Dictionary<Guid, PlayerPipeline>()
            {
                { _game.Player1.PlayerId, player1Pipeline },
                { _game.Player2.PlayerId, player2Pipeline }
            };
        }

        public void StartGame()
        {
            _game.StartGame();
            SendTurnNotification();
        }

        private void ReceiveAction(Guid playerId, GameAction action)
        {
            _game.TakeTurn(playerId, action);
            if (!_game.GameIsEnded)
                SendTurnNotification();
            else
            {
                SendEndedNotification();
                ClosePipelines();
            }
        }

        private void SendEndedNotification()
        {
            var notification = BuildNotification(_game.CurrentPlayer, _game.CurrentOpponent);
            notification.NotificationType = NotificationType.GameOver;
            _playerPipelines[_game.CurrentPlayer.PlayerId].Notifications.OnNext(notification);

            notification = BuildNotification(_game.CurrentOpponent, _game.CurrentPlayer);
            notification.NotificationType = NotificationType.GameOver;
            _playerPipelines[_game.CurrentOpponent.PlayerId].Notifications.OnNext(notification);

        }

        private void SendTurnNotification()
        {
            var notification = BuildNotification(_game.CurrentPlayer, _game.CurrentOpponent);
            notification.NotificationType = NotificationType.YourTurn;
            _playerPipelines[_game.CurrentPlayer.PlayerId].Notifications.OnNext(notification);
        }

        private void ClosePipelines()
        {
            //remove subscriptions
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            //Mark observers as completed
            foreach (var playerPipeline in _playerPipelines)
            {
                playerPipeline.Value.Notifications.OnCompleted();
            }
        }

        private Notification BuildNotification(IPlayerStatus self, IPlayerStatus opponent)
        {
            return new Notification()
            {
                GameId = GameId,
                PlayerId = self.PlayerId,
                CurrentStatus = self.Set,
                OppenentActiveCount = opponent.Set.Count(b => b == true)
            };
        }
    }
}