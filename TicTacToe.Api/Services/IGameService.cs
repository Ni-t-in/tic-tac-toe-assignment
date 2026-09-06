using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public interface IGameService
{
    Task<Game> CreateGameAsync(GameMode mode, int boardSize);
    Task<Game> GetGameAsync(Guid gameId);
    Task<Game> MakeMoveAsync(Guid gameId, Player player, int row, int column);
    Task<Game> UndoAsync(Guid gameId);
    Task<Game> ResetGameAsync(Guid gameId);
}
