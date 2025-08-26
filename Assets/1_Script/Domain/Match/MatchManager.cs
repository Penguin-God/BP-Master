
public class MatchManager
{
    readonly DraftActionController draftController;
    readonly PhaseManager phaseManager;
    readonly ActionEventBus eventBus = new ActionEventBus();

    readonly PhaseActionDispatcher blue;
    readonly PhaseActionDispatcher red;

    public MatchManager(PhaseManager phaseManager, DraftActionController draftController, PhaseActionDispatcher blueDispatcher, PhaseActionDispatcher redDispatcher)
    {
        this.phaseManager = phaseManager;
        this.draftController = draftController;

        this.blue = blueDispatcher;
        this.red = redDispatcher;

        this.draftController.OnActionDone += ProgressGame;
    }

    PhaseActionRequestor _blue;
    PhaseActionRequestor _red;
    public MatchManager(PhaseManager phaseManager, PhaseActionRequestor blueDispatcher, PhaseActionRequestor redDispatcher)
    {
        this.phaseManager = phaseManager;

        this._blue = blueDispatcher;
        this._red = redDispatcher;

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
