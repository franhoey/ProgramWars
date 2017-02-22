using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using ProgramWars.Server.Game;
using ProgramWars.Server.Game.Factories;
using ProgramWars.Server.Models;

namespace ProgramWars.Server.Control
{
    public class GameController
    {
        private readonly Dictionary<Guid, PlayerPipeline> _playerPipelines;

        private readonly IGame _game;

        public Guid GameId { get; }

        public GameController(IGame game, PlayerPipeline player1Pipeline, PlayerPipeline player2Pipeline)
        {
            GameId = Guid.NewGuid();
            _game = game;

            player1Pipeline.Actions.Subscribe(ReceiveAction);
            player2Pipeline.Actions.Subscribe(ReceiveAction);

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

        private void ReceiveAction(GameAction action)
        {
            _game.TakeTurn(action);
            if (!_game.GameIsEnded)
                SendTurnNotification();
            else
                SendEndedNotification();
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

        private Notification BuildNotification(IPlayerStatus self, IPlayerStatus opponent)
        {
            return new Notification()
            {
                GameId = GameId,
                PlayerId = self.PlayerId,
                CurrentStatus = self.Set,
                OppenentActiveCount = opponent.Set.Count(b => b = true)
            };
        }
    }
}