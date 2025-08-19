using System.Collections.Generic;

public class Phase
{
    GamePhase gamePhase;
    Queue<Team> teams;

    public Phase(GamePhase gamePhase, Queue<Team> teams)
    {
        this.gamePhase = gamePhase;
        this.teams = teams;
        CurrentTeam = teams.Peek();
    }

    public Team CurrentTeam { get; private set; }
    public bool IsDone => teams.Count == 0;

    public bool Next()
    {
        if(IsDone) return false;

        teams.Dequeue();
        if(IsDone == false)
            CurrentTeam = teams.Peek();
        return true;
    }
}
