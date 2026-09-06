using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public class ScoreBoardService : IScoreBoardService
{
    private readonly ScoreBoard _scoreBoard = new ScoreBoard();

    private readonly Dictionary<Guid, GameResult> _recordedGames = [];

    public ScoreBoard GetScoreBoard()
    {
        return _scoreBoard;
    }

    public void RecordGameResult(Game game)
    {
        if (game.GameStatus == GameStatus.InProgress)
        {
            return;
        }

        if (_recordedGames.ContainsKey(game.Id))
        {
            return;
        }

        var result = game.GameStatus == GameStatus.Won
            ? game.Winner == Player.X ? GameResult.XWin : GameResult.OWin
            : GameResult.Draw;

        switch (result)
        {
            case GameResult.XWin:
                _scoreBoard.XWins++;
                break;
            case GameResult.OWin:
                _scoreBoard.OWins++;
                break;
            case GameResult.Draw:
                _scoreBoard.Draws++;
                break;
        }

        _recordedGames[game.Id] = result;
    }

    public void RemoveGameResult(Guid gameId)
    {
        if (!_recordedGames.Remove(gameId, out var result))
        {
            return;
        }

        switch (result)
        {
            case GameResult.XWin:
                _scoreBoard.XWins--;
                break;
            case GameResult.OWin:
                _scoreBoard.OWins--;
                break;
            case GameResult.Draw:
                _scoreBoard.Draws--;
                break;
        }
    }

    public void ForgetGameResult(Guid gameId)
    {
        _recordedGames.Remove(gameId);
    }

    public void ResetScoreBoard()
    {
        _scoreBoard.XWins = 0;
        _scoreBoard.OWins = 0;
        _scoreBoard.Draws = 0;

        _recordedGames.Clear();
    }

    private enum GameResult
    {
        XWin,
        OWin,
        Draw
    }
}