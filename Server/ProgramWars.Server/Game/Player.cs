using System;
using System.Linq;

namespace ProgramWars.Server.Game
{
    public interface IPlayerStatus
    {
        Guid PlayerId { get; }
        bool[] Set { get; }
        bool IsDead { get; }
    }

    public interface IPlayer : IPlayerStatus
    {
        void ReceiveAttack(int position);
        void Repair(int position);
    }

    public class Player : IPlayer
    {
        private readonly bool[] _set;
        public Guid PlayerId { get; }

        public bool[] Set
            => (bool[]) _set.Clone();

        public bool IsDead
            => _set.All(s => s == false);

        public Player(int setSize)
        {
            _set = Enumerable.Repeat(true, setSize).ToArray();
            PlayerId = Guid.NewGuid();
        }

        public void ReceiveAttack(int position)
        {
            if(position == 0 || _set.Length < position)
                throw new ArgumentOutOfRangeException(nameof(position));

            _set[position - 1] = false;
        }

        public void Repair(int position)
        {
            if (position == 0 || _set.Length < position)
                throw new ArgumentOutOfRangeException(nameof(position));

            _set[position - 1] = true;
        }
    }
}