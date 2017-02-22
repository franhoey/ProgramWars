namespace ProgramWars.Server.Game.Factories
{
    public interface ITurnFactory
    {
        ITurn GetTurn(IPlayer currentPlayer, IPlayer opponent);
    }

    public class TurnFactory : ITurnFactory
    {
        public ITurn GetTurn(IPlayer currentPlayer, IPlayer opponent)
        {
            return new Turn(currentPlayer, opponent);
        }
    }
}