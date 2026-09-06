import type { Scoreboard as ScoreboardType } from "../types/game";

interface ScoreboardProps {
    scoreboard: ScoreboardType;
}

function Scoreboard({
    scoreboard,
}: ScoreboardProps) {
    return (
        <div className="panel scoreboard">
            <h2>Scoreboard</h2>

            <div className="scores">
                <div>
                    <strong>X</strong>
                    <span>{scoreboard.xWins}</span>
                </div>

                <div>
                    <strong>O</strong>
                    <span>{scoreboard.oWins}</span>
                </div>

                <div>
                    <strong>Draws</strong>
                    <span>{scoreboard.draws}</span>
                </div>
            </div>
        </div>
    );
}

export default Scoreboard;