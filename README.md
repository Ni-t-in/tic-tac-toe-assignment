# Tic Tac Toe

This is my Tic Tac Toe assignment. It has an ASP.NET Core API for the game rules and a React client for the user interface.

## What it does

- Supports two-player games.
- Supports playing against the computer.
- Supports 3x3, 4x4, and 5x5 boards.
- Shows the current game status and winning cells.
- Keeps a move history.
- Allows a move to be undone.
- Allows the current board to be reset without clearing the scoreboard.
- Keeps X wins, O wins, and draws for the current session.

## Project structure

- `TicTacToe.Api` - ASP.NET Core Web API, game engine, computer player, SQLite persistence, and API controllers.
- `TicTacToe.Tests` - tests for the game engine and scoreboard behavior.
- `tic-tac-toe-client` - React and TypeScript frontend built with Vite.

## How to run it

### Requirements

- .NET 10 SDK
- Node.js and npm

### Start the API

Run this from the repository root:

```bash
dotnet run --project ./TicTacToe.Api
```

The API runs on `http://localhost:8400`. Pending SQLite migrations are applied automatically when the API starts.

### Start the client

Open a second terminal:

```bash
cd tic-tac-toe-client
npm install
npm run dev
```

The client normally runs on `http://localhost:8399` and connects to the API on port `8400`.

To use a different API URL, set `VITE_API_URL` before starting the client.

## Tests and checks

From the repository root:

```bash
dotnet test ./TicTacToe.Tests
```

From `tic-tac-toe-client`:

```bash
npm run lint
npm run build
```

## Technical decisions

The game rules are kept in the API so the client does not decide whether a move is valid or whether a game has ended. The computer player uses the same game engine as a human player.

Games and their moves are stored in SQLite. A game is rebuilt from its saved move history when it is loaded. The scoreboard is kept in memory by the API, so it is not intended to be permanent storage.

## Assumptions and limitations

- The API and client are run locally during the assessment.
- The default CORS and API URLs are local development values.
- Scoreboard totals are reset when the API restarts or when a new browser session starts.
- There is no login or separate scoreboard for different users.
- The computer player uses a simple strategy: win when possible, block the player, choose the centre or a corner, then choose the first open cell.
