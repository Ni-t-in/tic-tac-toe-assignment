using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public interface IScoreBoardService
{
    ScoreBoard GetScoreBoard();
    void RecordGameResult(Game game);
    void RemoveGameResult(Guid gameId);
    void ForgetGameResult(Guid gameId);
    void ResetScoreBoard();
}