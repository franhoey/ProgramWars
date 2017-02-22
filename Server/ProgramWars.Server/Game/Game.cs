using System;
using System.Linq;
using ProgramWars.Server.Control;
using ProgramWars.Server.Game.Factories;

namespace ProgramWars.Server.Game
{
    public interface IGame
    {
        IPlayerStatus Player1 { get; }
        IPlayerStatus Player2 { get; }
        IPlayerStatus CurrentPlayer { get; }
        IPlayerStatus CurrentOpponent { get; }
        IPlayerStatus Winner { get; }
        bool GameIsEnded { get; }
        void StartGame();
        void TakeTurn(GameAction action);
    }

    public class Game : IGame
    {
        public const int DEFAULT_SET_SIZE = 3;

        private readonly ITurnFactory _turnFactory;

        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        public IPlayerStatus Player1
            => _player1;

        public IPlayerStatus Player2
            => _player2;


        public IPlayerStatus CurrentPlayer { get; private set; }
        public IPlayerStatus CurrentOpponent { get; private set; }

        public IPlayerStatus Winner
        {
            get
            {
                if(Player1.IsDead)
                    return Player2;

                if (Player2.IsDead)
                    return Player1;

                return null;
            }
        }

        public bool GameIsEnded
            => Winner != null;

        public Game() : this(DEFAULT_SET_SIZE)
        {
        }

        public Game(int setSize) : this(new PlayerFactory(), new TurnFactory(), setSize)
        {
        }

        public Game(IPlayerFactory playerFactory, ITurnFactory turnFactory, int setSize)
        {
            _turnFactory = turnFactory;

            _player1 = playerFactory.GetPlayer(setSize);
            _player2 = playerFactory.GetPlayer(setSize);
        }

        public void StartGame()
        {
            var rnd = new Random();

            if (rnd.Next(1, 2) == 1)
            {
                CurrentPlayer = _player1;
                CurrentOpponent = _player2;
            }
            else
            {
                CurrentPlayer = _player2;
                CurrentOpponent = _player1;
            }
        }

        public void TakeTurn(GameAction action)
        {
            if(GameIsEnded)
                throw new InvalidOperationException("Game has ended");

            if(CurrentPlayer == null)
                throw new InvalidOperationException("Game must be started before a turn can be taken");

            if(CurrentPlayer.PlayerId != action.PlayerId)
                throw new InvalidOperationException($"It is not the turn of player with id {action.PlayerId}");

            var turn = BuildTurn(action.PlayerId);
            turn.Action(action.Action, action.Position);

            if(!GameIsEnded)
                ChangeCurrentPlayer();
        }

        private ITurn BuildTurn(Guid playerId)
        {
            if(Player1.PlayerId == playerId)
                return _turnFactory.GetTurn(_player1, _player2);
            
            if (Player2.PlayerId == playerId)
                return _turnFactory.GetTurn(_player2, _player1);

            throw new ArgumentOutOfRangeException(nameof(playerId), "No player exists with the specified Id");
        }

        private void ChangeCurrentPlayer()
        {
            if (CurrentPlayer.PlayerId == Player1.PlayerId)
            {
                CurrentPlayer = Player2;
                CurrentOpponent = Player1;
            }
            else
            {
                CurrentPlayer = Player1;
                CurrentOpponent = Player2;
            }
        }
    }
}