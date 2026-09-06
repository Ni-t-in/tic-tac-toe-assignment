using Microsoft.EntityFrameworkCore;
using TicTacToe.Api.Data;
using TicTacToe.Api.Data.Entities;
using TicTacToe.Api.GameEngine;
using TicTacToe.Api.Models;

namespace TicTacToe.Api.Repositories;

public class GameRepository : IGameRepository
{
    private readonly TicTacToeDbContext _context;
    private readonly TicTacToeEngine _engine;

    public GameRepository(TicTacToeDbContext context, TicTacToeEngine engine)
    {
        _context = context;
        _engine = engine;
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Games
        .Include(game => game.Moves)
        .FirstOrDefaultAsync(game => game.Id == id);

        if (entity == null)
        {
            return null;
        }
        return MapToDomain(entity);
    }

    public async Task AddAsync(Game game)
    {
        var entity = MapToEntity(game);
        await _context.Games.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Game game)
    {
        var existingGame = await _context.Games
        .Include(existing => existing.Moves)
        .FirstOrDefaultAsync(existing => existing.Id == game.Id);

        if (existingGame == null)
        {
            throw new KeyNotFoundException($"Game with ID {game.Id} is not found.");
        }

        existingGame.BoardSize = game.BoardSize;
        existingGame.CurrentPlayer = game.CurrentPlayer;
        existingGame.GameMode = game.GameMode;
        existingGame.GameStatus = game.GameStatus;
        existingGame.Winner = game.Winner;

        _context.Moves.RemoveRange(existingGame.Moves);

        existingGame.Moves = game.MoveHistory
        .Select(move => new MoveEntity
        {
            MoveNumber = move.MoveNumber,
            Player = move.Player,
            Row = move.Row,
            Column = move.Column,
            GameId = game.Id
        }).ToList();

        await _context.SaveChangesAsync();
    }

    private Game MapToDomain(GameEntity gameEntity)
    {
        var game = new Game(gameEntity.Id, gameEntity.BoardSize)
        {
            CurrentPlayer = gameEntity.CurrentPlayer,
            GameMode = gameEntity.GameMode,
            GameStatus = gameEntity.GameStatus,
            Winner = gameEntity.Winner
        };

        foreach (var move in gameEntity.Moves.OrderBy(move => move.MoveNumber))
        {
            game.MoveHistory.Add(new Move
            {
                MoveNumber = move.MoveNumber,
                Player = move.Player,
                Row = move.Row,
                Column = move.Column
            });
            game.Board[(move.Row * game.BoardSize) + move.Column] = move.Player;
        }

        if (game.GameStatus == GameStatus.Won && game.Winner.HasValue)
        {
            game.WinningCells = _engine.FindWinningCells(game, game.Winner.Value);
        }

        return game;
    }

    private static GameEntity MapToEntity(Game game)
    {
        return new GameEntity
        {
            Id = game.Id,
            BoardSize = game.BoardSize,
            CurrentPlayer = game.CurrentPlayer,
            GameMode = game.GameMode,
            GameStatus = game.GameStatus,
            Winner = game.Winner,

            Moves = game.MoveHistory.Select(move => new MoveEntity
            {
                MoveNumber = move.MoveNumber,
                Player = move.Player,
                Row = move.Row,
                Column = move.Column,
                GameId = game.Id
            }).ToList()
        };
    }
}