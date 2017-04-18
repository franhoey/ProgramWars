using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using ProgramWars.Server.Control;
using ProgramWars.Server.Game;
using ProgramWars.Server.Models;

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
        public void When_GameStarted_CurrentPlayerReceivesANotification()
        {
            var currentPlayer = new Mock<IPlayer>();
            currentPlayer.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var opponent = new Mock<IPlayer>();
            opponent.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var gameMock = new Mock<IGame>();
            gameMock.Setup(m => m.Player1)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.Player2)
                .Returns(opponent.Object);
            gameMock.Setup(m => m.CurrentPlayer)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.CurrentOpponent)
                .Returns(opponent.Object);

            var currentPlayerPipeline = new PlayerPiplineTestWrapper();
            var opponentPipeline = new PlayerPiplineTestWrapper();

            var controller = new GameController(gameMock.Object, currentPlayerPipeline.Pipeline, opponentPipeline.Pipeline);
            controller.StartGame();
            
            Assert.AreEqual(1, currentPlayerPipeline.Notifications.Count);
            Assert.True(currentPlayerPipeline.Notifications.TrueForAll(notification => notification.NotificationType == NotificationType.YourTurn));
        }

        [Test]
        public void When_PlayerMoves_ANotificationIsTriggered()
        {
            var currentPlayer = new Mock<IPlayer>();
            currentPlayer.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var opponent = new Mock<IPlayer>();
            opponent.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var gameMock = new Mock<IGame>();
            gameMock.Setup(m => m.Player1)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.Player2)
                .Returns(opponent.Object);
            gameMock.Setup(m => m.CurrentPlayer)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.CurrentOpponent)
                .Returns(opponent.Object);

            var currentPlayerPipeline = new PlayerPiplineTestWrapper();
            var opponentPipeline = new PlayerPiplineTestWrapper();

            var controller = new GameController(gameMock.Object, currentPlayerPipeline.Pipeline, opponentPipeline.Pipeline);
            controller.StartGame();

            currentPlayerPipeline.SendAction(new GameAction());

            Assert.AreEqual(2, currentPlayerPipeline.Notifications.Count);
            Assert.True(currentPlayerPipeline.Notifications.All(notification => notification.NotificationType == NotificationType.YourTurn));
        }

        [Test]
        public void When_PlayerMoves_IfGameIsEnded_ANotificationIsTriggeredToBothPlayers()
        {
            var currentPlayer = new Mock<IPlayer>();
            currentPlayer.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var opponent = new Mock<IPlayer>();
            opponent.Setup(m => m.PlayerId).Returns(Guid.NewGuid());
            var gameMock = new Mock<IGame>();
            gameMock.Setup(m => m.Player1)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.Player2)
                .Returns(opponent.Object);
            gameMock.Setup(m => m.CurrentPlayer)
                .Returns(currentPlayer.Object);
            gameMock.Setup(m => m.CurrentOpponent)
                .Returns(opponent.Object);
            gameMock.Setup(m => m.GameIsEnded)
                .Returns(true);

            var currentPlayerPipeline = new PlayerPiplineTestWrapper();
            var opponentPipeline = new PlayerPiplineTestWrapper();

            var controller = new GameController(gameMock.Object, currentPlayerPipeline.Pipeline, opponentPipeline.Pipeline);
            controller.StartGame();

            currentPlayerPipeline.SendAction(new GameAction());

            Assert.AreEqual(2, currentPlayerPipeline.Notifications.Count);
            Assert.True(currentPlayerPipeline.Notifications.Any(notification => notification.NotificationType == NotificationType.GameOver));

            Assert.AreEqual(1, opponentPipeline.Notifications.Count);
            Assert.True(opponentPipeline.Notifications.Any(notification => notification.NotificationType == NotificationType.GameOver));
        }
    }
}