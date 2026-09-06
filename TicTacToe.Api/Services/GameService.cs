using TicTacToe.Api.GameEngine;
using TicTacToe.Api.Models;
using TicTacToe.Api.Repositories;

namespace TicTacToe.Api.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly TicTacToeEngine _gameEngine;
    private readonly IScoreBoardService _scoreBoardService;
    private readonly ComputerPlayer _computerPlayer;

    public GameService(IGameRepository gameRepository, TicTacToeEngine gameEngine, IScoreBoardService scoreBoardService, ComputerPlayer computerPlayer)
    {
        _gameRepository = gameRepository;
        _gameEngine = gameEngine;
        _scoreBoardService = scoreBoardService;
        _computerPlayer = computerPlayer;
    }

    public async Task<Game> CreateGameAsync(GameMode gameMode, int boardSize)
    {
        if (boardSize is < 3 or > 5)
        {
            throw new ArgumentException("Board size must be between 3 and 5.", nameof(boardSize));
        }

        var game = new Game(boardSize)
        {
            GameMode = gameMode
        };

        await _gameRepository.AddAsync(game);

        return game;
    }

    public async Task<Game> GetGameAsync(Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId);

        if (game == null)
        {
            throw new KeyNotFoundException($"Game with ID {gameId} was not found.");
        }

        return game;
    }

    public async Task<Game> MakeMoveAsync(Guid gameId, Player player, int row, int column)
    {
        var game = await GetGameAsync(gameId);

        _gameEngine.MakeMove(game, player, row, column);

        if (game.GameStatus != GameStatus.InProgress)
        {
            _scoreBoardService.RecordGameResult(game);
            await _gameRepository.UpdateAsync(game);

            return game;
        }

        if (game.GameMode == GameMode.Computer &&
            game.CurrentPlayer == Player.O)
        {
            var computerMove = _computerPlayer.GetMove(game);

            _gameEngine.MakeMove(
                game,
                Player.O,
                computerMove.Row,
                computerMove.Column);

            if (game.GameStatus != GameStatus.InProgress)
            {
                _scoreBoardService.RecordGameResult(game);
            }
        }

        await _gameRepository.UpdateAsync(game);

        return game;
    }

    public async Task<Game> UndoAsync(Guid gameId)
    {
        var game = await GetGameAsync(gameId);

        _scoreBoardService.RemoveGameResult(game.Id);
        _gameEngine.UndoMove(game);

        await _gameRepository.UpdateAsync(game);

        return game;
    }

    public async Task<Game> ResetGameAsync(Guid gameId)
    {
        var game = await GetGameAsync(gameId);

        _scoreBoardService.ForgetGameResult(game.Id);
        _gameEngine.ResetGame(game);

        await _gameRepository.UpdateAsync(game);

        return game;
    }
}