using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TicTacToe.Api.Data;
using TicTacToe.Api.GameEngine;
using TicTacToe.Api.Repositories;
using TicTacToe.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TicTacToeDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<TicTacToeEngine>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddSingleton<IScoreBoardService, ScoreBoardService>();
builder.Services.AddScoped<ComputerPlayer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins(builder.Configuration["ClientUrl"] ?? "http://localhost:8399")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.WebHost.UseUrls("http://localhost:8400");
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
    db.Database.Migrate();
}

app.UseRouting();
app.UseExceptionHandler("/error");
app.UseCors("ReactClient");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Tic Tac Toe API v1");
    });
}
app.MapControllers();
app.Run();
