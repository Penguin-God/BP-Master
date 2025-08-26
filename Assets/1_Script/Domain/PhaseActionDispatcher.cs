
public class PhaseActionRequestor
{
    readonly Team Team;
    readonly IActionHandler matchActionHandler;
    public PhaseActionRequestor(Team team, IActionHandler matchActionClient)
    {
        Team = team;
        this.matchActionHandler = matchActionClient;
    }

    public void OnRequestAction(ActionEventBus draftAction, GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Ban: matchActionHandler.OnRequestBan(Team, draftAction); break;
            case GamePhase.Pick: matchActionHandler.OnRequestPick(Team, draftAction); break;
            case GamePhase.Swap: matchActionHandler.OnRequestSwap(Team, draftAction); break;
        }
    }
}

public interface IActionHandler
{
    public void OnRequestBan(Team team, ActionEventBus draftAction);
    public void OnRequestPick(Team team, ActionEventBus draftAction);
    public void OnRequestSwap(Team team, ActionEventBus draftAction);
}

