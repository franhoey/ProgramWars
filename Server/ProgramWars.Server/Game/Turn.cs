namespace ProgramWars.Server.Game
{
    public interface ITurn
    {
        IPlayer CurrentPlayer { get; }
        IPlayer Opponent { get; }
        void Action(ActionType actionType, int position);
    }

    public class Turn : ITurn
    {
        public IPlayer CurrentPlayer { get; }
        public IPlayer Opponent { get; }

        public Turn(IPlayer currentPlayer, IPlayer opponent)
        {
            CurrentPlayer = currentPlayer;
            Opponent = opponent;
        }

        public void Action(ActionType actionType, int position)
        {
            switch (actionType)
            {
                case ActionType.Attack:
                {
                    Attack(position);
                    break;
                }
                case ActionType.Repair:
                {
                    Repair(position);
                    break;
                }
            }
        }

        private void Attack(int position)
        {
            Opponent.ReceiveAttack(position);
        }

        private void Repair(int position)
        {
            CurrentPlayer.Repair(position);
        }
    }
}