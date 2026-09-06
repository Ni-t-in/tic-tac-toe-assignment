using TicTacToe.Api.Models;

namespace TicTacToe.Api.GameEngine;

public class TicTacToeEngine
{
    public void MakeMove(Game game, Player player, int row, int column)
    {
        ValidateMove(game, player, row, column);

        game.Board[(row * game.BoardSize) + column] = player;

        game.MoveHistory.Add(new Move
        {
            MoveNumber = game.MoveHistory.Count + 1,
            Player = player,
            Row = row,
            Column = column
        });

        var isGameOver = CheckGameStatus(game);

        if (!isGameOver)
        {
            game.CurrentPlayer = player == Player.X ? Player.O : Player.X;
        }
    }

    private void ValidateMove(Game game, Player player, int row, int column)
    {
        if (row < 0 || row >= game.BoardSize || column < 0 || column >= game.BoardSize)
        {
            throw new ArgumentException("Invalid board position.");
        }

        if (game.GameStatus != GameStatus.InProgress)
        {
            throw new InvalidOperationException("The game is already complete.");
        }

        if (player != game.CurrentPlayer)
        {
            throw new InvalidOperationException("It is not this player's turn.");
        }

        if (game.Board[(row * game.BoardSize) + column] != null)
        {
            throw new InvalidOperationException("The selected cell is already occupied.");
        }
    }

    private bool CheckGameStatus(Game game)
    {
        var winningCells = FindWinningCells(game, game.CurrentPlayer);

        if (winningCells.Count > 0)
        {
            game.GameStatus = GameStatus.Won;
            game.Winner = game.CurrentPlayer;
            game.WinningCells = winningCells;

            return true;
        }

        for (var index = 0; index < game.Board.Length; index++)
        {
            if (game.Board[index] == null)
            {
                game.GameStatus = GameStatus.InProgress;
                return false;
            }
        }

        game.GameStatus = GameStatus.Draw;
        return true;
    }

    public List<int> FindWinningCells(Game game, Player player)
    {
        for (var row = 0; row < game.BoardSize; row++)
        {
            var cells = new List<int>();

            for (var column = 0; column < game.BoardSize; column++)
            {
                var index = (row * game.BoardSize) + column;

                if (game.Board[index] != player)
                {
                    break;
                }

                cells.Add(index);
            }

            if (cells.Count == game.BoardSize)
                return cells;
        }

        for (var column = 0; column < game.BoardSize; column++)
        {
            var cells = new List<int>();

            for (var row = 0; row < game.BoardSize; row++)
            {
                var index = (row * game.BoardSize) + column;

                if (game.Board[index] != player)
                {
                    break;
                }

                cells.Add(index);
            }

            if (cells.Count == game.BoardSize)
                return cells;
        }

        var diagonal = new List<int>();

        for (var i = 0; i < game.BoardSize; i++)
        {
            var index = (i * game.BoardSize) + i;

            if (game.Board[index] != player)
                break;

            diagonal.Add(index);
        }

        if (diagonal.Count == game.BoardSize)
            return diagonal;

        diagonal.Clear();

        for (var i = 0; i < game.BoardSize; i++)
        {
            var index = (i * game.BoardSize) + (game.BoardSize - 1 - i);

            if (game.Board[index] != player)
                break;

            diagonal.Add(index);
        }

        if (diagonal.Count == game.BoardSize)
            return diagonal;

        return [];
    }

    public void ResetGame(Game game)
    {
        game.Board = new Player?[game.BoardSize * game.BoardSize];
        game.CurrentPlayer = Player.X;
        game.GameStatus = GameStatus.InProgress;
        game.Winner = null;
        game.WinningCells.Clear();
        game.MoveHistory.Clear();
    }

    public void UndoMove(Game game)
    {
        if (game.MoveHistory.Count == 0)
        {
            throw new InvalidOperationException("There are no new moves to undo.");
        }

        var movesToRemove = game.GameMode == GameMode.Computer ? 2 : 1;
        movesToRemove = Math.Min(movesToRemove, game.MoveHistory.Count);
        game.MoveHistory.RemoveRange(game.MoveHistory.Count - movesToRemove, movesToRemove);
        RebuildBoard(game);
        game.GameStatus = GameStatus.InProgress;
        game.Winner = null;
        game.WinningCells.Clear();

        RestoreCurrentPlayer(game);
    }

    private void RebuildBoard(Game game)
    {
        game.Board = new Player?[game.BoardSize * game.BoardSize];

        foreach (var move in game.MoveHistory)
        {
            game.Board[(move.Row * game.BoardSize) + move.Column] = move.Player;
        }
    }

    private void RestoreCurrentPlayer(Game game)
    {
        if (game.MoveHistory.Count == 0)
        {
            game.CurrentPlayer = Player.X;
            return;
        }

        var lastMove = game.MoveHistory[^1];

        game.CurrentPlayer = lastMove.Player == Player.X ? Player.O : Player.X;
    }
}