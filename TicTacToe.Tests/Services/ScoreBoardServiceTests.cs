using TicTacToe.Api.Models;
using TicTacToe.Api.Services;

namespace TicTacToe.Tests.Services;

public class ScoreBoardServiceTests
{
    [Fact]
    public void RecordGameResult_DoesNotCountTheSameGameTwice()
    {
        var service = new ScoreBoardService();
        var game = new Game
        {
            GameStatus = GameStatus.Won,
            Winner = Player.X
        };

        service.RecordGameResult(game);
        service.RecordGameResult(game);

        Assert.Equal(1, service.GetScoreBoard().XWins);
        Assert.Equal(0, service.GetScoreBoard().OWins);
        Assert.Equal(0, service.GetScoreBoard().Draws);
    }

    [Fact]
    public void RemoveGameResult_AllowsTheGameToBeCountedAgain()
    {
        var service = new ScoreBoardService();
        var game = new Game
        {
            GameStatus = GameStatus.Won,
            Winner = Player.X
        };

        service.RecordGameResult(game);
        service.RemoveGameResult(game.Id);
        service.RecordGameResult(game);

        Assert.Equal(1, service.GetScoreBoard().XWins);
    }

    [Fact]
    public void RemoveGameResult_RemovesADraw()
    {
        var service = new ScoreBoardService();
        var game = new Game
        {
            GameStatus = GameStatus.Draw
        };

        service.RecordGameResult(game);
        service.RemoveGameResult(game.Id);

        Assert.Equal(0, service.GetScoreBoard().Draws);
    }

    [Fact]
    public void ForgetGameResult_KeepsTheScoreAndAllowsAnotherRound()
    {
        var service = new ScoreBoardService();
        var game = new Game
        {
            GameStatus = GameStatus.Won,
            Winner = Player.X
        };

        service.RecordGameResult(game);
        service.ForgetGameResult(game.Id);
        service.RecordGameResult(game);

        Assert.Equal(2, service.GetScoreBoard().XWins);
    }
}
