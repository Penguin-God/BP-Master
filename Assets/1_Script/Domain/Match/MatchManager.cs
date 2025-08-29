
public class MatchManager
{
    readonly PhaseManager phaseManager;
    PhaseActionRequestor _blue;
    PhaseActionRequestor _red;
    public MatchManager(PhaseManager phaseManager, PhaseActionRequestor blueDispatcher, PhaseActionRequestor redDispatcher)
    {
        this.phaseManager = phaseManager;
        this._blue = blueDispatcher;
        this._red = redDispatcher;
        phaseManager.OnFlowChanged += UpdateFlow;
    }

    void ProgressGame()
    {
        switch (CurrentTurn)
        {
            case Team.Blue:
                _blue.OnRequestAction(CurrentPhase); break;
            case Team.Red:
                _red.OnRequestAction(CurrentPhase); break;
            case Team.All:
                _blue.OnRequestAction(CurrentPhase);
                _red.OnRequestAction(CurrentPhase);
                break;
        }
    }

    GameFlowData currentFlow;

    void UpdateFlow(GameFlowData flow)
    {
        currentFlow = flow;
        ProgressGame();
    }

    public Team CurrentTurn => currentFlow.Turn;
    public GamePhase CurrentPhase => currentFlow.Phase;

    public void GameStart() => phaseManager.Start();
}
