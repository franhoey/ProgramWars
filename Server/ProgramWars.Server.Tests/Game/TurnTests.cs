using Moq;
using NUnit.Framework;
using ProgramWars.Server.Game;

namespace ProgramWars.Server.Tests.Game
{
    [TestFixture]
    public class TurnTests
    {
        [Test]
        public void When_Attack_ThenOpponentIsAttacked()
        {
            const int position = 1;

            var current = new Mock<IPlayer>();
            var opponent = new Mock<IPlayer>();
            var turn = new Turn(current.Object, opponent.Object);
            
            turn.Action(ActionType.Attack, 1);

            opponent.Verify(m => m.ReceiveAttack(position), Times.Once);
            opponent.Verify(m => m.Repair(position), Times.Never);

            current.Verify(m => m.ReceiveAttack(position), Times.Never);
            current.Verify(m => m.Repair(position), Times.Never);
        }

        [Test]
        public void When_Repair_ThenCurrentPlayerIsRepaired()
        {
            const int position = 1;

            var current = new Mock<IPlayer>();
            var opponent = new Mock<IPlayer>();
            var turn = new Turn(current.Object, opponent.Object);

            turn.Action(ActionType.Repair, 1);

            opponent.Verify(m => m.ReceiveAttack(It.IsAny<int>()), Times.Never);
            opponent.Verify(m => m.Repair(It.IsAny<int>()), Times.Never);

            current.Verify(m => m.ReceiveAttack(It.IsAny<int>()), Times.Never);
            current.Verify(m => m.Repair(position), Times.Once);
        }
    }
}