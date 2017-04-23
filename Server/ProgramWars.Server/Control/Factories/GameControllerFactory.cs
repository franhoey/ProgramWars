using ProgramWars.Server.Game;

namespace ProgramWars.Server.Control.Factories
{
    public interface IGameControllerFactory
    {
        IGameController GetGameController(IGame game, PlayerPipeline player1Pipeline, PlayerPipeline player2Pipeline);
    }

    public class GameControllerFactory : IGameControllerFactory
    {
        public IGameController GetGameController(IGame game, PlayerPipeline player1Pipeline, PlayerPipeline player2Pipeline)
        {
            return new GameController(game, player1Pipeline, player2Pipeline);
        }
    }
}