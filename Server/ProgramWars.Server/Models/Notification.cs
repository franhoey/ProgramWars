using System;

namespace ProgramWars.Server.Models
{
    public class Notification
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool[] CurrentStatus { get; set; }
        public int OppenentActiveCount { get; set; }
    }
}