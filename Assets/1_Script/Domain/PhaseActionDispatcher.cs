
public class PhaseActionRequestor
{
    readonly Team Team;
    readonly IActionHandler matchActionHandler;
    public PhaseActionRequestor(Team team, IActionHandler matchActionClient)
    {
        Team = team;
        this.matchActionHandler = matchActionClient;
    }

    public void OnRequestAction(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Ban: matchActionHandler.OnRequestBan(Team); break;
            case GamePhase.Pick: matchActionHandler.OnRequestPick(Team); break;
            case GamePhase.Swap: matchActionHandler.OnRequestSwap(Team); break;
            case GamePhase.Trait: matchActionHandler.OnRequestActive(Team); break;
        }
    }
}

public interface IActionHandler
{
    public void OnRequestBan(Team team);
    public void OnRequestPick(Team team);
    public void OnRequestSwap(Team team);
    public void OnRequestActive(Team team);
}

