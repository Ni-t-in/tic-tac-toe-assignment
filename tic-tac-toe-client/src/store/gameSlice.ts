import {
    createSlice,
    type PayloadAction,
} from "@reduxjs/toolkit";

import type {
    Game,
    Scoreboard,
} from "../types/game";

interface GameState {
    game: Game | null;
    scoreboard: Scoreboard;
    loading: boolean;
    error: string | null;
}

const initialState: GameState = {
    game: null,

    scoreboard: {
        xWins: 0,
        oWins: 0,
        draws: 0,
    },

    loading: false,
    error: null,
};

const gameSlice = createSlice({
    name: "game",

    initialState,

    reducers: {
        setGame(
            state,
            action: PayloadAction<Game | null>
        ) {
            state.game = action.payload;
            state.error = null;
        },

        setScoreboard(
            state,
            action: PayloadAction<Scoreboard>
        ) {
            state.scoreboard = action.payload;
        },

        setLoading(
            state,
            action: PayloadAction<boolean>
        ) {
            state.loading = action.payload;
        },

        setError(
            state,
            action: PayloadAction<string | null>
        ) {
            state.error = action.payload;
        },
    },
});

export const {
    setGame,
    setScoreboard,
    setLoading,
    setError,
} = gameSlice.actions;

export default gameSlice.reducer;