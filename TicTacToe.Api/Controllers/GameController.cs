using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Models;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public async Task<ActionResult<Game>> CreateGameAsync([FromQuery] GameMode gameMode, [FromQuery] int boardSize = 3)
    {
        var game = await _gameService.CreateGameAsync(gameMode, boardSize);

        return Ok(game);
    }

    [HttpGet("{gameId:guid}")]
    public async Task<ActionResult<Game>> GetByIdAsync(Guid gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);

        return Ok(game);
    }

    [HttpPost("{gameId:guid}/moves")]
    public async Task<ActionResult<Game>> MakeMoveAsync(Guid gameId, MakeMoveRequest request)
    {
        var game = await _gameService.MakeMoveAsync(gameId, request.Player, request.Row, request.Column);

        return Ok(game);
    }

    [HttpPost("{gameId:guid}/undo")]
    public async Task<ActionResult<Game>> UndoAsync(Guid gameId)
    {
        var game = await _gameService.UndoAsync(gameId);

        return Ok(game);
    }

    [HttpPost("{gameId:guid}/reset")]
    public async Task<ActionResult<Game>> ResetGameAsync(Guid gameId)
    {
        var game = await _gameService.ResetGameAsync(gameId);

        return Ok(game);
    }
}