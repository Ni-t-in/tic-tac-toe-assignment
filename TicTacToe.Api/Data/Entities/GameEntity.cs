using TicTacToe.Api.Models;

namespace TicTacToe.Api.Data.Entities;

public class GameEntity
{
    public Guid Id { get; set; }
    public int BoardSize { get; set; }
    public Player CurrentPlayer { get; set; }
    public GameMode GameMode { get; set; }
    public GameStatus GameStatus { get; set; }
    public Player? Winner { get; set; }

    public List<MoveEntity> Moves { get; set; } = [];
}