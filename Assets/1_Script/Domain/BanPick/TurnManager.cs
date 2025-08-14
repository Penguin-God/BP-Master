using System.Collections.Generic;

public class TurnManager
{
    Queue<Team> teams = new();
    public TurnManager(IReadOnlyList<Team> teams)
    {
        this.teams = new Queue<Team>(teams);
        CurrentTeam = this.teams.Dequeue();
    }

    public Team CurrentTeam { get; private set; } = Team.Blue;
    
    public Team NextTurn()
    {
        if (teams.Count == 0) return CurrentTeam;

        CurrentTeam = teams.Dequeue();
        return CurrentTeam;
    }
}
