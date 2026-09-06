using Microsoft.EntityFrameworkCore;
using TicTacToe.Api.Data.Entities;

namespace TicTacToe.Api.Data;

public class TicTacToeDbContext : DbContext
{
    public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options) : base(options)
    {
    }

    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<MoveEntity> Moves => Set<MoveEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameEntity>(entity =>
        {
            entity.HasKey(game => game.Id);
            entity.HasMany(game => game.Moves)
            .WithOne(move => move.Game)
            .HasForeignKey(move => move.GameId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MoveEntity>(entity =>
        {
            entity.HasKey(move => move.Id);
        });
    }


}