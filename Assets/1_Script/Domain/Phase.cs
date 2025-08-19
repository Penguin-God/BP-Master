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

    public bool Next()
    {
        if(IsDone) return false;

        actionTeams.Dequeue();
        if(IsDone == false)
            CurrentTeam = actionTeams.Peek();
        return true;
    }
}
