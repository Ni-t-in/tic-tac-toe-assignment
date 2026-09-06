import type { Move } from "../types/game";

interface MoveHistoryProps {
    moves: Move[];
}

function MoveHistory({
    moves,
}: MoveHistoryProps) {
    return (
        <div className="panel">
            <h2>Move History</h2>

            {moves.length === 0 ? (
                <p>No moves yet.</p>
            ) : (
                <ol>
                    {moves.map((move) => (
                        <li key={move.moveNumber}>
                            Move {move.moveNumber}: Player {move.player}
                            {" → "}
                            ({move.row + 1}, {move.column + 1})
                        </li>
                    ))}
                </ol>
            )}
        </div>
    );
}

export default MoveHistory;