

using System;
using System.Collections.Generic;

public class Phase
{
    GamePhase gamePhase;
    Queue<Team> teams;

    public Phase(GamePhase gamePhase, Queue<Team> teams)
    {
        this.gamePhase = gamePhase;
        this.teams = teams;
    }

    public GamePhase CurrentTeam { get; set; }
    public bool IsDone { get; private set; } = false;

    public bool Next()
    {
        return false;
    }
}
