

public class PhaseActionDispatcher
{
    readonly Team Team;
    readonly IDraftActionHandler matchActionClient;
    public PhaseActionDispatcher(Team team, IDraftActionHandler matchActionClient)
    {
        Team = team;
        this.matchActionClient = matchActionClient;
    }

    public void OnRequestAction(DraftActionController draftAction, GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Ban: matchActionClient.OnRequestBan(Team, draftAction); break;
            case GamePhase.Pick: matchActionClient.OnRequestPick(Team, draftAction); break;
            case GamePhase.Swap: matchActionClient.OnRequestSwap(Team, draftAction); break;
        }
    }
}

public interface IDraftActionHandler
{
    public void OnRequestBan(Team team, DraftActionController draftAction);
    public void OnRequestPick(Team team, DraftActionController draftAction);
    public void OnRequestSwap(Team team, DraftActionController draftAction);
}

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

