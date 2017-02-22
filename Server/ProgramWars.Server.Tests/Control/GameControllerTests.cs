using Moq;
using NUnit.Framework;
using ProgramWars.Server.Control;
using ProgramWars.Server.Game;

namespace ProgramWars.Server.Tests.Control
{
    [TestFixture]
    public class GameControllerTests
    {
        [Test]
        public void When_GameStarted_ActualGameIsStarted()
        {
            var player1 = new Player(3);
            var player2 = new Player(3);
            var gameMock = new Mock<IGame>();
            gameMock.Setup(m => m.Player1)
                .Returns(player1);
            gameMock.Setup(m => m.Player2)
                .Returns(player2);
            gameMock.Setup(m => m.CurrentPlayer)
                .Returns(player1);
            gameMock.Setup(m => m.CurrentOpponent)
                .Returns(player2);

            var player1Pipeline = new PlayerPiplineTestWrapper();
            var player2Pipeline = new PlayerPiplineTestWrapper();

            var controller = new GameController(gameMock.Object, player1Pipeline.Pipeline, player2Pipeline.Pipeline);
            controller.StartGame();

            gameMock.Verify(m => m.StartGame(), Times.Once);
        }

        [Test]
        public void When_GameStarted_APlayerReceivesANotification()
        {
            var player1 = new Player(3);
            var player2 = new Player(3);
            var gameMock = new Mock<IGame>();
            gameMock.Setup(m => m.Player1)
                .Returns(player1);
            gameMock.Setup(m => m.Player2)
                .Returns(player2);
            gameMock.Setup(m => m.CurrentPlayer)
                .Returns(player1);
            gameMock.Setup(m => m.CurrentOpponent)
                .Returns(player2);

            var player1Pipeline = new PlayerPiplineTestWrapper();
            var player2Pipeline = new PlayerPiplineTestWrapper();

            var controller = new GameController(gameMock.Object, player1Pipeline.Pipeline, player2Pipeline.Pipeline);
            controller.StartGame();

            var allNotifications = player1Pipeline.Notifications;
            allNotifications.AddRange(player2Pipeline.Notifications);

            Assert.AreEqual(1, allNotifications.Count);
        }
    }
}