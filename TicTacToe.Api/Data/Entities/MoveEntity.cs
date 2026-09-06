using TicTacToe.Api.Models;

namespace TicTacToe.Api.Data.Entities;

public class MoveEntity
{
    public int Id { get; set; }
    public int MoveNumber { get; set; }
    public Player Player { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public Guid GameId { get; set; }

    public GameEntity Game { get; set; } = null!;
}