using System;
using System.Collections.Generic;

public class PhaseData
{
    readonly public GamePhase GamePhase;
    readonly public Phase Phase;

    public PhaseData(GamePhase gamePhase, Phase phase)
    {
        GamePhase = gamePhase;
        Phase = phase;
    }
}

public class Phase
{
    Queue<Team> actionTeams;
    public bool HasNext => actionTeams.Count > 0;
    public bool IsDone => actionTeams.Count == 0;

    public Phase(IEnumerable<Team> teams) => this.actionTeams = new Queue<Team>(teams);
    
    
    public Team GetNext()
    {
        if (actionTeams.Count == 0)
            throw new InvalidOperationException("턴이 없는데 턴을 달래");
        return actionTeams.Dequeue();
    }

    public Team PeekNext()
    {
        if (actionTeams.Count == 0)
            throw new InvalidOperationException("턴이 없는데 턴을 달래");
        return actionTeams.Peek();
    }
}
