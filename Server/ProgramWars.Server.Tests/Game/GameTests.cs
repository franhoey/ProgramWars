using System;
using Moq;
using NUnit.Framework;
using ProgramWars.Server.Control;
using ProgramWars.Server.Game;
using ProgramWars.Server.Game.Factories;

namespace ProgramWars.Server.Tests.Game
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void When_TakesTurn_BeforeGameStarted_ExceptionIsThrown()
        {
            const int setSize = 3;
            var player1 = new Player(setSize);
            var player2 = new Player(setSize);

            var turnFactory = new Mock<ITurnFactory>();

            var playerFactory = new Mock<IPlayerFactory>();
            playerFactory.SetupSequence(m => m.GetPlayer(It.IsAny<int>()))
                .Returns(player1)
                .Returns(player2);

            var game = new Server.Game.Game(playerFactory.Object, turnFactory.Object, setSize);

            Assert.Throws<InvalidOperationException>(
                () => game.TakeTurn(new GameAction() { GameId = player1.PlayerId, Action = ActionType.Attack, Position = 1}),
                "Game must be started before a turn can be taken");
        }

        [Test]
        public void When_TakesTurn_WithIncorrectPlayer_ExceptionIsThrown()
        {
            const int setSize = 3;
            var player1 = new Player(setSize);
            var player2 = new Player(setSize);

            var turnFactory = new Mock<ITurnFactory>();

            var playerFactory = new Mock<IPlayerFactory>();
            playerFactory.SetupSequence(m => m.GetPlayer(It.IsAny<int>()))
                .Returns(player1)
                .Returns(player2);

            var game = new Server.Game.Game(playerFactory.Object, turnFactory.Object, setSize);
            game.StartGame();
            
            var opponent = (game.CurrentPlayer.PlayerId == player1.PlayerId) ? player2 : player1;

            Assert.Throws<InvalidOperationException>(
                () => game.TakeTurn(new GameAction() {PlayerId = opponent.PlayerId, Action = ActionType.Attack, Position = 1}),
                "It is not the turn of player with id {0}",
                opponent.PlayerId);
        }

        [Test]
        public void When_TakesTurn_SetupPlayersInCorrectPositionsAndTakesCorrectTurn()
        {
            const int setSize = 3;
            var player1 = new Player(setSize);
            var player2 = new Player(setSize);
            var turnMock = new Mock<ITurn>();

            var turnFactory = new Mock<ITurnFactory>();
            turnFactory.Setup(m => m.GetTurn(It.IsAny<IPlayer>(), It.IsAny<IPlayer>()))
                .Returns(turnMock.Object);

            var playerFactory = new Mock<IPlayerFactory>();
            playerFactory.SetupSequence(m => m.GetPlayer(It.IsAny<int>()))
                .Returns(player1)
                .Returns(player2);

            var game = new Server.Game.Game(playerFactory.Object, turnFactory.Object, setSize);
            
            game.StartGame();

            var currentPlayer = (game.CurrentPlayer.PlayerId == player1.PlayerId) ? player1 : player2;
            var opponent = (game.CurrentPlayer.PlayerId == player1.PlayerId) ? player2 : player1;

            game.TakeTurn(new GameAction() {PlayerId = currentPlayer.PlayerId, Action = ActionType.Attack, Position = 1});
            
            turnFactory.Verify(m => m.GetTurn(currentPlayer, opponent), Times.Once);
            turnMock.Verify(m => m.Action(ActionType.Attack, 1), Times.Once);
        }

        [Test]
        public void When_TakesTurn_NextPlayerIsSetAsCurrentPlayer()
        {
            const int setSize = 3;
            var player1 = new Player(setSize);
            var player2 = new Player(setSize);
            var turnMock = new Mock<ITurn>();

            var turnFactory = new Mock<ITurnFactory>();
            turnFactory.Setup(m => m.GetTurn(It.IsAny<IPlayer>(), It.IsAny<IPlayer>()))
                .Returns(turnMock.Object);

            var playerFactory = new Mock<IPlayerFactory>();
            playerFactory.SetupSequence(m => m.GetPlayer(It.IsAny<int>()))
                .Returns(player1)
                .Returns(player2);

            var game = new Server.Game.Game(playerFactory.Object, turnFactory.Object, setSize);

            game.StartGame();

            var currentPlayer = (game.CurrentPlayer.PlayerId == player1.PlayerId) ? player1 : player2;
            var opponent = (game.CurrentPlayer.PlayerId == player1.PlayerId) ? player2 : player1;

            game.TakeTurn(new GameAction() {PlayerId = currentPlayer.PlayerId, Action = ActionType.Attack, Position = 1});

            Assert.AreEqual(opponent.PlayerId, game.CurrentPlayer.PlayerId);
        }

        [Test]
        public void When_PlayerIsDead_WinnerIsPopulated()
        {
            const int setSize = 3;
            var player1 = new Player(setSize);
            player1.ReceiveAttack(1);
            player1.ReceiveAttack(2);
            player1.ReceiveAttack(3);
            var player2 = new Player(setSize);
            var turnMock = new Mock<ITurn>();

            var turnFactory = new Mock<ITurnFactory>();
            turnFactory.Setup(m => m.GetTurn(It.IsAny<IPlayer>(), It.IsAny<IPlayer>()))
                .Returns(turnMock.Object);

            var playerFactory = new Mock<IPlayerFactory>();
            playerFactory.SetupSequence(m => m.GetPlayer(It.IsAny<int>()))
                .Returns(player1)
                .Returns(player2);

            var game = new Server.Game.Game(playerFactory.Object, turnFactory.Object, setSize);
            
            Assert.True(game.GameIsEnded);
            Assert.AreEqual(player2.PlayerId, game.Winner.PlayerId);
        }
    }
}