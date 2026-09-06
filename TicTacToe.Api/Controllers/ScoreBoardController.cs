using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Models;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/ScoreBoard")]
public class ScoreBoardController : ControllerBase
{
    private readonly IScoreBoardService _scoreBoardService;

    public ScoreBoardController(IScoreBoardService scoreBoardService)
    {
        _scoreBoardService = scoreBoardService;
    }

    [HttpGet]
    public ActionResult<ScoreBoard> GetScoreBoard()
    {
        return Ok(_scoreBoardService.GetScoreBoard());
    }

    [HttpPost("reset")]
    public ActionResult<ScoreBoard> ResetScoreBoard()
    {
        _scoreBoardService.ResetScoreBoard();

        return Ok(_scoreBoardService.GetScoreBoard());
    }
}