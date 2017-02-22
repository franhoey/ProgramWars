namespace ProgramWars.Server.Game.Factories
{
    public interface IPlayerFactory
    {
        IPlayer GetPlayer(int setSize);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer GetPlayer(int setSize)
        {
            return new Player(setSize);
        }
    }
}