namespace TicTacToe.Api.Models;

public class Game
{
    public Guid Id { get; private set; }
    public int BoardSize { get; set; }
    public Player?[] Board { get; set; }
    public Player CurrentPlayer { get; set; } = Player.X;
    public GameMode GameMode { get; set; }
    public GameStatus GameStatus { get; set; } = GameStatus.InProgress;
    public Player? Winner { get; set; }
    public List<int> WinningCells { get; set; } = [];
    public List<Move> MoveHistory { get; set; } = [];

    public Game(Guid id, int boardSize = 3)
    {
        Id = id;

        BoardSize = boardSize;

        Board = new Player?[boardSize * boardSize];
    }

    public Game(int boardSize = 3) : this(Guid.NewGuid(), boardSize)
    {
    }
}