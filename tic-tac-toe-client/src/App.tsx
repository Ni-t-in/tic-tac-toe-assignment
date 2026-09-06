import { useEffect, useState } from "react";

import Board from "./components/Board";
import MoveHistory from "./components/MoveHistory";
import Scoreboard from "./components/Scoreboard";

import {
  createGame,
  makeMove,
  undoMove,
  resetGame,
  getScoreboard,
  resetScoreboard,
} from "./services/gameApi";

import {
  setGame,
  setScoreboard,
  setLoading,
  setError,
} from "./store/gameSlice";

import {
  useAppDispatch,
  useAppSelector,
} from "./store/hooks";
import type { AppDispatch } from "./store/store";

import type { GameMode } from "./types/game";

import "./App.css";

async function loadScoreboard(dispatch: AppDispatch) {
  try {
    const data = await getScoreboard();
    dispatch(setScoreboard(data));
  } catch (error) {
    dispatch(
      setError(
        error instanceof Error
          ? error.message
          : "Failed to load scoreboard."
      )
    );
  }
}

function App() {
  const dispatch = useAppDispatch();

  const {
    game,
    scoreboard,
    loading,
    error,
  } = useAppSelector((state) => state.game);

  const [gameMode, setGameMode] =
    useState<GameMode>("TwoPlayer");

  const [boardSize, setBoardSize] = useState(3);
  const [lastStartedMode, setLastStartedMode] =
    useState<GameMode | null>(null);

  useEffect(() => {
    void resetScoreboard()
      .then((data) => dispatch(setScoreboard(data)))
      .catch((error: unknown) => {
        dispatch(
          setError(
            error instanceof Error
              ? error.message
              : "Failed to reset scoreboard."
          )
        );
      });
  }, [dispatch]);

  async function handleCreateGame() {
    try {
      dispatch(setLoading(true));
      dispatch(setError(null));

      const newGame = await createGame(gameMode, boardSize);

      if (lastStartedMode && lastStartedMode !== gameMode) {
        const resetScores = await resetScoreboard();
        dispatch(setScoreboard(resetScores));
      }

      setLastStartedMode(gameMode);
      dispatch(setGame(newGame));
    } catch (error) {
      dispatch(
        setError(
          error instanceof Error
            ? error.message
            : "Failed to create game."
        )
      );
    } finally {
      dispatch(setLoading(false));
    }
  }

  async function handleCellClick(index: number) {
    if (!game) {
      return;
    }

    if (game.gameStatus !== "InProgress") {
      return;
    }

    const row = Math.floor(index / game.boardSize);
    const column = index % game.boardSize;

    try {
      dispatch(setLoading(true));
      dispatch(setError(null));

      const updatedGame = await makeMove(
        game.id,
        game.currentPlayer,
        row,
        column
      );

      dispatch(setGame(updatedGame));

      if (updatedGame.gameStatus !== "InProgress") {
        await loadScoreboard(dispatch);
      }
    } catch (error) {
      dispatch(
        setError(
          error instanceof Error
            ? error.message
            : "Invalid move."
        )
      );
    } finally {
      dispatch(setLoading(false));
    }
  }

  async function handleUndo() {
    if (!game) {
      return;
    }

    if (game.moveHistory.length === 0) {
      return;
    }

    if (game.gameStatus !== "InProgress") {
      return;
    }

    try {
      dispatch(setLoading(true));
      dispatch(setError(null));

      const updatedGame = await undoMove(game.id);

      dispatch(setGame(updatedGame));
    } catch (error) {
      dispatch(
        setError(
          error instanceof Error
            ? error.message
            : "Failed to undo move."
        )
      );
    } finally {
      dispatch(setLoading(false));
    }
  }

  async function handleResetGame() {
    if (!game) {
      return;
    }

    try {
      dispatch(setLoading(true));
      dispatch(setError(null));

      const reset = await resetGame(game.id);

      dispatch(setGame(reset));
    } catch (error) {
      dispatch(
        setError(
          error instanceof Error
            ? error.message
            : "Failed to reset game."
        )
      );
    } finally {
      dispatch(setLoading(false));
    }
  }

  function handleNewGame() {
    dispatch(setGame(null));
    dispatch(setError(null));
  }

  async function handleResetScoreboard() {
    try {
      dispatch(setLoading(true));
      dispatch(setError(null));

      const resetScores = await resetScoreboard();

      dispatch(setScoreboard(resetScores));
    } catch (error) {
      dispatch(
        setError(
          error instanceof Error
            ? error.message
            : "Failed to reset scoreboard."
        )
      );
    } finally {
      dispatch(setLoading(false));
    }
  }

  return (
    <div className="app">
      <h1>Tic Tac Toe</h1>

      <div className="game-controls">

        <select
          value={gameMode}
          onChange={(event) =>
            setGameMode(
              event.target.value as GameMode
            )
          }
          disabled={loading}
        >
          <option value="TwoPlayer">
            Two Player
          </option>

          <option value="Computer">
            vs Computer
          </option>
        </select>

        <select
          value={boardSize}
          onChange={(e) => setBoardSize(Number(e.target.value))}
        >
          <option value={3}>3 × 3</option>
          <option value={4}>4 × 4</option>
          <option value={5}>5 × 5</option>
        </select>

        {!game && (
          <button
            onClick={handleCreateGame}
            disabled={loading}
          >
            Start Game
          </button>
        )}

        {game && (
          <button
            onClick={handleNewGame}
            disabled={loading}
          >
            New Game
          </button>
        )}
      </div>

      {error && (
        <div className="error">
          {error}
        </div>
      )}

      {game && (
        <div className="game-container">

          <div className="game-info">

            <p>
              Mode:{" "}
              {game.gameMode === "Computer"
                ? "vs Computer"
                : "Two Player"}
            </p>

            {game.gameStatus === "InProgress" && (
              <h2>
                Player {game.currentPlayer}'s Turn
              </h2>
            )}

            {game.gameStatus === "Won" && (
              <h2>
                🎉 Player {game.winner} Wins!
              </h2>
            )}

            {game.gameStatus === "Draw" && (
              <h2>
                🤝 It's a Draw!
              </h2>
            )}

          </div>

          <Board
            game={game}
            onCellClick={handleCellClick}
          />

          <div className="buttons">

            <button
              onClick={handleUndo}
              disabled={
                loading ||
                game.moveHistory.length === 0 ||
                game.gameStatus !== "InProgress"
              }
            >
              Undo
            </button>

            <button
              onClick={handleResetGame}
              disabled={loading}
            >
              Reset Game
            </button>

          </div>

          <MoveHistory
            moves={game.moveHistory}
          />

        </div>
      )}

      <Scoreboard
        scoreboard={scoreboard}
      />

      <button
        onClick={handleResetScoreboard}
        disabled={loading}
      >
        Reset Scoreboard
      </button>
    </div>
  );
}

export default App;