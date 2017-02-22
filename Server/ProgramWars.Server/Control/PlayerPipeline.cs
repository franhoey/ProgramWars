using System;
using ProgramWars.Server.Models;

namespace ProgramWars.Server.Control
{
    public class PlayerPipeline
    {
        public IObservable<GameAction> Actions { get; }
        public IObserver<Notification> Notifications { get; }

        public PlayerPipeline(IObservable<GameAction> actions, IObserver<Notification> notifications)
        {
            Actions = actions;
            Notifications = notifications;
        }
    }
}