import type {
    Game,
    GameMode,
    Player,
    Scoreboard,
} from "../types/game";

const API_BASE_URL = `${import.meta.env.VITE_API_URL ?? "http://localhost:8400"}/api`;

async function handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
        const error = await response.json().catch(() => null);

        throw new Error(
            error?.message ?? "Something went wrong with the API request."
        );
    }

    return response.json();
}

export async function createGame(gameMode: GameMode, boardSize: number): Promise<Game> {
    const response = await fetch(
        `${API_BASE_URL}/game?gameMode=${gameMode}&boardSize=${boardSize}`,
        {
            method: "POST",
        }
    );

    return handleResponse<Game>(response);
}

export async function getGame(gameId: string): Promise<Game> {
    const response = await fetch(
        `${API_BASE_URL}/game/${gameId}`
    );

    return handleResponse<Game>(response);
}

export async function makeMove(
    gameId: string,
    player: Player,
    row: number,
    column: number
): Promise<Game> {
    const response = await fetch(
        `${API_BASE_URL}/game/${gameId}/moves`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                player,
                row,
                column,
            }),
        }
    );

    return handleResponse<Game>(response);
}

export async function undoMove(gameId: string): Promise<Game> {
    const response = await fetch(
        `${API_BASE_URL}/game/${gameId}/undo`,
        {
            method: "POST",
        }
    );

    return handleResponse<Game>(response);
}

export async function resetGame(gameId: string): Promise<Game> {
    const response = await fetch(
        `${API_BASE_URL}/game/${gameId}/reset`,
        {
            method: "POST",
        }
    );

    return handleResponse<Game>(response);
}

export async function getScoreboard(): Promise<Scoreboard> {
    const response = await fetch(
        `${API_BASE_URL}/scoreboard`
    );

    return handleResponse<Scoreboard>(response);
}

export async function resetScoreboard(): Promise<Scoreboard> {
    const response = await fetch(
        `${API_BASE_URL}/scoreboard/reset`,
        {
            method: "POST",
        }
    );

    return handleResponse<Scoreboard>(response);
}