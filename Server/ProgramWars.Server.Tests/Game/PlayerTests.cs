using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using ProgramWars.Server.Game;

namespace ProgramWars.Server.Tests.Game
{
    [TestFixture]
    public class PlayerTests
    {
        [TestCase(5)]
        [TestCase(3)]
        public void When_Created_HasSetOfCorrectSizeAndValues(int setSize)
        {
            var player = new Player(setSize);

            Assert.AreEqual(setSize, player.Set.Length);
            Assert.True(player.Set.All(i => i == true));
        }

        [Test]
        public void When_Attacked_CorrectSetValueIsChanged()
        {
            var player = new Player(3);
            player.ReceiveAttack(1);
            
            Assert.False(player.Set[0]);
            Assert.True(player.Set[1]);
            Assert.True(player.Set[2]);
        }

        [TestCase(3, 4)]
        [TestCase(3, 0)]
        public void When_Attacked_WithInvalidPosition_ThrowsException(int setSize, int attackPostition)
        {
            var player = new Player(setSize);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                    player.ReceiveAttack(attackPostition));
            
        }

        [Test]
        public void When_Repaired_CorrectSetValueIsChanged()
        {
            var player = new Player(3);
            player.ReceiveAttack(1);
            player.Repair(1);

            Assert.True(player.Set.All(i => i == true));
        }

        [TestCase(3, 4)]
        [TestCase(3, 0)]
        public void When_Repaired_WithInvalidPosition_ThrowsException(int setSize, int repairPostition)
        {
            var player = new Player(setSize);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                    player.Repair(repairPostition));

        }
    
        [Test]
        public void When_WholeSetIsAttacked_IsDead()
        {
            var player = new Player(3);

            player.ReceiveAttack(1);
            player.ReceiveAttack(2);
            player.ReceiveAttack(3);

            Assert.True(player.IsDead);
        }
    }
}