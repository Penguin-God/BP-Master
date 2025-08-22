using System;
using System.Collections.Generic;

public class Phase
{
    Queue<Team> actionTeams;

    public Phase(IEnumerable<Team> teams)
    {
        this.actionTeams = new Queue<Team>(teams);
    }

    public bool IsDone => actionTeams.Count == 0;
    
    public Team GetNext()
    {
        if(actionTeams.Count == 0)
            throw new InvalidOperationException("턴이 없는데 턴을 달래");

        return actionTeams.Dequeue();
    }
}
