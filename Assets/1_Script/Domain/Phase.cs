using System;
using System.Collections.Generic;

public class Phase
{
    Queue<Team> actionTeams;

    public Phase(IEnumerable<Team> teams)
    {
        this.actionTeams = new Queue<Team>(teams);
        CurrentTeam = this.actionTeams.Peek();
    }

    public Team CurrentTeam { get; private set; }
    public bool IsDone => actionTeams.Count == 0;

    public Team GetNext()
    {
        if(actionTeams.Count == 0)
            throw new InvalidOperationException("턴이 없는데 턴을 달래");

        return actionTeams.Dequeue();
    }

    public bool Next()
    {
        if(IsDone) return false;

        actionTeams.Dequeue();
        if(IsDone == false)
            CurrentTeam = actionTeams.Peek();
        return true;
    }
}
