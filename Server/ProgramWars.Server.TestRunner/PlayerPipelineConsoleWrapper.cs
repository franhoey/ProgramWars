using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using ProgramWars.Server.Control;
using ProgramWars.Server.Models;

namespace ProgramWars.Server.TestRunner
{
    public class PlayerPipelineConsoleWrapper
    {
        private readonly Subject<Notification> _notificationErrors;
        private readonly Subject<GameAction> _actions;
        private readonly ConsoleColor _colour;

        public PlayerPipeline Pipeline { get; }

        public Guid PlayerId { get; set; }

        public List<Notification> Notifications { get; }

        public PlayerPipelineConsoleWrapper(ConsoleColor colour)
        {
            _colour = colour;
            _notificationErrors = new Subject<Notification>();
            _actions = new Subject<GameAction>();

            _notificationErrors.Subscribe(x => DisplayNotification(x));

            Pipeline = new PlayerPipeline(_actions, _notificationErrors);
        }

        private void DisplayNotification(Notification notification)
        {
            Console.ForegroundColor = _colour;
            Console.WriteLine($"Notification: {notification.NotificationType}");
            Console.WriteLine($"\tYou:\t\t\t{string.Join(",", notification.CurrentStatus)}");
            Console.WriteLine($"\tOpponent Active:\t{notification.OppenentActiveCount}");
        }

        public void SendAction(GameAction action)
        {
            _actions.OnNext(action);
        }
    }
}