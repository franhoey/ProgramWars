using System.Collections.Concurrent;
using ProgramWars.Server.Control.Factories;
using ProgramWars.Server.Game.Factories;

namespace ProgramWars.Server.Control
{
    public class Lobby
    {
        private readonly ConcurrentQueue<PlayerPipeline> _waitingPlayers = new ConcurrentQueue<PlayerPipeline>();
        private readonly ConcurrentBag<IGameController> _runningGames =new ConcurrentBag<IGameController>();

        private readonly IGameFactory _gameFactory;
        private readonly IGameControllerFactory _gameControllerFactory;

        public Lobby(IGameControllerFactory gameControllerFactory, IGameFactory gameFactory)
        {
            _gameControllerFactory = gameControllerFactory;
            _gameFactory = gameFactory;
        }

        public void RequestGame(PlayerPipeline playerPipeline)
        {
            PlayerPipeline opponent;
            if (_waitingPlayers.TryDequeue(out opponent))
            {
                CreateGame(opponent, playerPipeline);
                return;
            }

            //If no waiting players add player to wait list
            _waitingPlayers.Enqueue(playerPipeline);
        }

        public void CreateGame(PlayerPipeline player1, PlayerPipeline player2)
        {
            var controller = _gameControllerFactory.GetGameController(
                _gameFactory.Game(),
                player1,
                player2);
            controller.StartGame();
            _runningGames.Add(controller);
            
        }
    }
}