using TicTacToe.Api.Models;
using TicTacToe.Api.GameEngine;

namespace TicTacToe.Tests.GameEngine;

public class TicTacToeEngineTests
{
    [Fact]
    public void MakeMove_ValidMove_ShouldPlacePlayerOnBoard()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // Act
        engine.MakeMove(game, Player.X, 0, 0);

        // Assert
        Assert.Equal(Player.X, game.Board[0]);
    }

    [Fact]
    public void MakeMove_ValidMove_ShouldSwitchTurn()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // Act
        engine.MakeMove(game, Player.X, 0, 0);

        // Assert
        Assert.Equal(Player.O, game.CurrentPlayer);
    }

    [Fact]
    public void MakeMove_OccupiedCell_ShouldThrowException()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            engine.MakeMove(game, Player.O, 0, 0));
    }

    [Fact]
    public void MakeMove_WrongPlayer_ShouldThrowException()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // X starts the game, so O cannot move first.

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            engine.MakeMove(game, Player.O, 0, 0));
    }

    [Fact]
    public void MakeMove_WhenPlayerCompletesRow_ShouldWin()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // X → (0,0)
        engine.MakeMove(game, Player.X, 0, 0);

        // O → (1,0)
        engine.MakeMove(game, Player.O, 1, 0);

        // X → (0,1)
        engine.MakeMove(game, Player.X, 0, 1);

        // O → (1,1)
        engine.MakeMove(game, Player.O, 1, 1);

        // Act
        // X completes the first row.
        engine.MakeMove(game, Player.X, 0, 2);

        // Assert
        Assert.Equal(GameStatus.Won, game.GameStatus);
        Assert.Equal(Player.X, game.Winner);

        // The frontend will use these indexes to highlight the winning cells.
        Assert.Equal([0, 1, 2], game.WinningCells);
    }

    [Fact]
    public void MakeMove_WhenPlayerCompletesColumn_ShouldWin()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 0, 1);

        engine.MakeMove(game, Player.X, 1, 0);
        engine.MakeMove(game, Player.O, 1, 1);

        // Act
        // X completes the first column.
        engine.MakeMove(game, Player.X, 2, 0);

        // Assert
        Assert.Equal(GameStatus.Won, game.GameStatus);
        Assert.Equal(Player.X, game.Winner);

        Assert.Equal([0, 3, 6], game.WinningCells);
    }

    [Fact]
    public void MakeMove_WhenPlayerCompletesDiagonalTopLeftToBottomRight_ShouldWin()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 0, 1);

        engine.MakeMove(game, Player.X, 1, 1);
        engine.MakeMove(game, Player.O, 1, 0);

        // Act
        // X completes the top-left to bottom-right diagonal.
        engine.MakeMove(game, Player.X, 2, 2);

        // Assert
        Assert.Equal(GameStatus.Won, game.GameStatus);
        Assert.Equal(Player.X, game.Winner);

        Assert.Equal([0, 4, 8], game.WinningCells);
    }

    [Fact]
    public void MakeMove_WhenPlayerCompletesDiagonalTopRightToBottomLeft_ShouldWin()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 2);
        engine.MakeMove(game, Player.O, 0, 1);

        engine.MakeMove(game, Player.X, 1, 1);
        engine.MakeMove(game, Player.O, 1, 0);

        // Act
        // X completes the top-right to bottom-left diagonal.
        engine.MakeMove(game, Player.X, 2, 0);

        // Assert
        Assert.Equal(GameStatus.Won, game.GameStatus);
        Assert.Equal(Player.X, game.Winner);

        Assert.Equal([2, 4, 6], game.WinningCells);
    }

    [Fact]
    public void MakeMove_WhenBoardIsFullWithoutWinner_ShouldDraw()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // X O X
        // X O O
        // O X X
        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 0, 1);
        engine.MakeMove(game, Player.X, 0, 2);

        engine.MakeMove(game, Player.O, 1, 1);
        engine.MakeMove(game, Player.X, 1, 0);
        engine.MakeMove(game, Player.O, 1, 2);

        engine.MakeMove(game, Player.X, 2, 1);
        engine.MakeMove(game, Player.O, 2, 0);

        // Act
        engine.MakeMove(game, Player.X, 2, 2);

        // Assert
        Assert.Equal(GameStatus.Draw, game.GameStatus);
        Assert.Null(game.Winner);
    }

    [Fact]
    public void MakeMove_AfterGameIsCompleted_ShouldThrowException()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // X wins the first row.
        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 1, 0);
        engine.MakeMove(game, Player.X, 0, 1);
        engine.MakeMove(game, Player.O, 1, 1);
        engine.MakeMove(game, Player.X, 0, 2);

        // Act & Assert
        // The game is already won, so no additional move is allowed.
        Assert.Throws<InvalidOperationException>(() =>
            engine.MakeMove(game, Player.O, 2, 2));
    }

    [Fact]
    public void ResetGame_ShouldRestoreInitialGameState()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 1, 1);

        // Act
        engine.ResetGame(game);

        // Assert
        Assert.Equal(Player.X, game.CurrentPlayer);
        Assert.Equal(GameStatus.InProgress, game.GameStatus);
        Assert.Null(game.Winner);

        Assert.Empty(game.MoveHistory);
        Assert.Empty(game.WinningCells);

        // The board should contain no moves.
        for (var row = 0; row < game.BoardSize; row++)
        {
            for (var column = 0; column < game.BoardSize; column++)
            {
                Assert.Null(game.Board[(row*game.BoardSize) + column]);
            }
        }
    }

    [Fact]
    public void Undo_TwoPlayerMode_ShouldRemoveLastMove()
    {
        // Arrange
        var game = new Game
        {
            GameMode = GameMode.TwoPlayer
        };

        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 1, 1);

        // Act
        engine.UndoMove(game);

        // Assert
        Assert.Equal(Player.X, game.Board[0]);
        Assert.Null(game.Board[1]);

        Assert.Single(game.MoveHistory);
        Assert.Equal(Player.X, game.MoveHistory[0].Player);

        // O's move was removed, so it is O's turn again.
        Assert.Equal(Player.O, game.CurrentPlayer);

        Assert.Equal(GameStatus.InProgress, game.GameStatus);
    }

    [Fact]
    public void Undo_ComputerMode_ShouldRemoveLastTwoMoves()
    {
        // Arrange
        var game = new Game
        {
            GameMode = GameMode.Computer
        };

        var engine = new TicTacToeEngine();

        engine.MakeMove(game, Player.X, 0, 0);
        engine.MakeMove(game, Player.O, 1, 1);

        // Act
        engine.UndoMove(game);

        // Assert
        Assert.Null(game.Board[0]);
        Assert.Null(game.Board[1]);

        Assert.Empty(game.MoveHistory);

        // Both moves were removed, so X starts again.
        Assert.Equal(Player.X, game.CurrentPlayer);

        Assert.Equal(GameStatus.InProgress, game.GameStatus);
    }

    [Fact]
    public void Undo_WhenThereAreNoMoves_ShouldThrowException()
    {
        // Arrange
        var game = new Game();
        var engine = new TicTacToeEngine();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            engine.UndoMove(game));
    }
}