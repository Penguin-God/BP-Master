
public class MatchManager
{
    readonly PhaseManager phaseManager;
    readonly ActionEventBus eventBus;

    PhaseActionRequestor _blue;
    PhaseActionRequestor _red;
    public MatchManager(PhaseManager phaseManager, ActionEventBus eventBus, PhaseActionRequestor blueDispatcher, PhaseActionRequestor redDispatcher)
    {
        this.phaseManager = phaseManager;

        this._blue = blueDispatcher;
        this._red = redDispatcher;
        this.eventBus = eventBus;
        eventBus.OnActionDone += ProgressGame;
    }

    void ProgressGame()
    {
        currentFlow = phaseManager.GetNextFlow();
        eventBus.ChangeTeam(CurrentTurn);

        switch (CurrentTurn)
        {
            case Team.Blue:
                _blue.OnRequestAction(eventBus, CurrentPhase); break;
            case Team.Red:
                _red.OnRequestAction(eventBus, CurrentPhase); break;
            case Team.All:
                _blue.OnRequestAction(eventBus, CurrentPhase);
                _red.OnRequestAction(eventBus, CurrentPhase);
                break;
        }
    }

    GameFlowData currentFlow;
    public Team CurrentTurn => currentFlow.Turn;
    public GamePhase CurrentPhase => currentFlow.Phase;

    public void GameStart() => ProgressGame();
}
