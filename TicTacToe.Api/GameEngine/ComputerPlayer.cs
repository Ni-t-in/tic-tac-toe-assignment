using TicTacToe.Api.Models;

namespace TicTacToe.Api.GameEngine;

public class ComputerPlayer
{
    private readonly TicTacToeEngine _engine;

    public ComputerPlayer(TicTacToeEngine engine)
    {
        _engine = engine;
    }

    public (int Row, int Column) GetMove(Game game)
    {
        var computer = Player.O;
        var human = Player.X;

        var winningMove = FindWinningMove(game, computer);

        if (winningMove.HasValue)
            return winningMove.Value;

        var blockingMove = FindWinningMove(game, human);

        if (blockingMove.HasValue)
            return blockingMove.Value;

        var center = game.BoardSize / 2;

        if (IsEmpty(game, center, center))
            return (center, center);

        var corners = new[]
        {
            (0, 0),
            (0, game.BoardSize - 1),
            (game.BoardSize - 1, 0),
            (game.BoardSize - 1, game.BoardSize - 1)
        };

        foreach (var corner in corners)
        {
            if (IsEmpty(game, corner.Item1, corner.Item2))
                return corner;
        }

        for (var row = 0; row < game.BoardSize; row++)
        {
            for (var column = 0; column < game.BoardSize; column++)
            {
                if (IsEmpty(game, row, column))
                    return (row, column);
            }
        }

        throw new InvalidOperationException("No available moves.");
    }

    private (int Row, int Column)? FindWinningMove(
        Game game,
        Player player)
    {
        for (var row = 0; row < game.BoardSize; row++)
        {
            for (var column = 0; column < game.BoardSize; column++)
            {
                if (!IsEmpty(game, row, column))
                    continue;

                var index = (row * game.BoardSize) + column;

                game.Board[index] = player;

                var wouldWin = _engine.FindWinningCells(game, player).Count > 0;

                game.Board[index] = null;

                if (wouldWin)
                    return (row, column);
            }
        }

        return null;
    }

    private static bool IsEmpty(Game game, int row, int column)
    {
        var index = (row * game.BoardSize) + column;

        return game.Board[index] == null;
    }
}