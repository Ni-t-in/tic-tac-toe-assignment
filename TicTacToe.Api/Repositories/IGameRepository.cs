using TicTacToe.Api.Models;

namespace TicTacToe.Api.Repositories;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(Guid id);
    Task AddAsync(Game game);
    Task UpdateAsync(Game game);
}
