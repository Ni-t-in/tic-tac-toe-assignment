import type { Player } from "../types/game";

interface CellProps {
    value: Player | null;
    isWinningCell: boolean;
    onClick: () => void;
}

function Cell({
    value,
    isWinningCell,
    onClick,
}: CellProps) {
    return (
        <button
            className={`cell ${isWinningCell ? "winning-cell" : ""}`}
            onClick={onClick}
            disabled={value !== null}
        >
            {value}
        </button>
    );
}

export default Cell;