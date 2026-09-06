export type Player = "X" | "O";

export type GameMode = "TwoPlayer" | "Computer";

export type GameStatus = "InProgress" | "Won" | "Draw";

export interface Move {
    moveNumber: number;
    player: Player;
    row: number;
    column: number;
}

export interface Game {
    id: string;
    boardSize: number;
    board: (Player | null)[];
    currentPlayer: Player;
    gameMode: GameMode;
    gameStatus: GameStatus;
    winner: Player | null;
    winningCells: number[];
    moveHistory: Move[];
}

export interface Scoreboard {
    xWins: number;
    oWins: number;
    draws: number;
}