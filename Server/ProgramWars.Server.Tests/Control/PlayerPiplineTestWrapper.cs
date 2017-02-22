using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using NUnit.Framework;
using ProgramWars.Server.Control;
using ProgramWars.Server.Models;

namespace ProgramWars.Server.Tests.Control
{
    public class PlayerPiplineTestWrapper
    {
        private readonly Subject<Notification> _notificationErrors;
        private readonly Subject<GameAction> _actions;

        public PlayerPipeline Pipeline { get; }

        public List<Notification> Notifications { get; }

        public PlayerPiplineTestWrapper()
        {
            Notifications = new List<Notification>();
            _notificationErrors = new Subject<Notification>();
            _actions = new Subject<GameAction>();

            _notificationErrors.Subscribe(x => Notifications.Add(x));

            Pipeline = new PlayerPipeline(_actions, _notificationErrors);
        }

        public void SendAction(GameAction action)
        {
            _actions.OnNext(action);
        }
    }
}