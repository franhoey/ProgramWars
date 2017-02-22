using System;
using ProgramWars.Server.Game;

namespace ProgramWars.Server.Control
{
    public class GameAction
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public ActionType Action { get; set; }
        public int Position { get; set; }
    }
}