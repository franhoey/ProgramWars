using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramWars.Server.Control;
using ProgramWars.Server.Game;

namespace ProgramWars.Server.TestRunner
{
    class Program
    {
        private static readonly PlayerPipelineConsoleWrapper Player1 = new PlayerPipelineConsoleWrapper(ConsoleColor.Cyan);
        private static readonly PlayerPipelineConsoleWrapper Player2 = new PlayerPipelineConsoleWrapper(ConsoleColor.Green);
        static void Main(string[] args)
        {
            Console.WriteLine("Example commands ");
            Console.WriteLine("player1 attack 2");
            Console.WriteLine("player1 repair 3");
            Console.WriteLine("exit");

            Console.WriteLine();
            
            var gameController = new GameController(new Game.Game(3), Player1.Pipeline, Player2.Pipeline);
            gameController.StartGame();
            
            RunGame();
        }

        private static void RunGame()
        {
            var command = Console.ReadLine();
            if (command.ToLower() == "exit")
                return;

            try
            {

                var action = new GameAction();

                var commandParts = command.Trim().Split(' ');
                var player = ParsePlayer(commandParts[0]);

                action.Action = ParseAction(commandParts[1]);
                action.Position = ParsePosition(commandParts[2]);

                player.SendAction(action);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                RunGame();
            }

            RunGame();
        }

        private static PlayerPipelineConsoleWrapper ParsePlayer(string playerString)
        {
            if(playerString.ToLower() == "player1")
                return Player1;

            if (playerString.ToLower() == "player2")
                return Player2;

            throw new ArgumentException("Could not parse player");
        }

        private static ActionType ParseAction(string actionString)
        {
            if (actionString.ToLower() == "attack")
                return ActionType.Attack;

            if (actionString.ToLower() == "repair")
                return ActionType.Repair;

            throw new ArgumentException("Could not parse action");
        }

        private static int ParsePosition(string positionString)
        {
            int position;
            if(int.TryParse(positionString, out position))
                return position;
            
            throw new ArgumentException("Could not parse position");
        }
    }
}
