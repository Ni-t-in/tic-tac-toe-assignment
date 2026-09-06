import type { Game } from "../types/game";
import Cell from "./Cell";

interface BoardProps {
    game: Game;
    onCellClick: (index: number) => void;
}

function Board({ game, onCellClick }: BoardProps) {
    const board = game.board;

    return (
        <div
            className="board"
            style={{
                gridTemplateColumns: `repeat(${game.boardSize}, 1fr)`,
                gridTemplateRows: `repeat(${game.boardSize}, 1fr)`
            }}
        >
            {board.map((value, index) => (
                <Cell
                    key={index}
                    value={value}
                    isWinningCell={game.winningCells.includes(index)}
                    onClick={() => onCellClick(index)}
                />
            ))}
        </div>
    );
}

export default Board;