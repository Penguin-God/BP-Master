public interface IPhaseEntry
{
    void EnterBan();
    void EnterPick();
}

public class TeamPhaseEntryDispatcher
{
    readonly IPhaseEntry blue;
    readonly IPhaseEntry red;

    public TeamPhaseEntryDispatcher(IPhaseEntry blue, IPhaseEntry red)
    {
        this.blue = blue;
        this.red = red;
    }

    public void EnterPhase(GameFlowData flow)
    {
        var entry = GetEntry(flow.Turn);

        switch (flow.Phase)
        {
            case GamePhase.Ban: entry.EnterBan(); break;
            case GamePhase.Pick:entry.EnterPick(); break;
        }
    }

    IPhaseEntry GetEntry(Team team)
    {
        if (team == Team.Blue) return blue;
        return red;
    }
}
