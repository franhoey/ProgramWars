namespace ProgramWars.Server.Game.Factories
{
    public interface IGameFactory
    {
        IGame Game();
        IGame Game(int setSize);
    }

    public class GameFactory : IGameFactory
    {
        public IGame Game() 
        {
            return new Game();
        }

        public IGame Game(int setSize)
        {
            return new Game(setSize);
        }        
    }
}