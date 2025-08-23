
public class MatchManager
{
    readonly DraftActionController draftController;
    readonly PhaseManager phaseManager;

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

    void ProgressGame()
    {
        currentFlow = phaseManager.GetNextFlow();
        draftController.ChangePhase(CurrentPhase, CurrentTurn);

        switch (CurrentTurn)
        {
            case Team.Blue:
                blue.OnRequestAction(draftController, CurrentPhase); break;
            case Team.Red:
                red.OnRequestAction(draftController, CurrentPhase); break;
            case Team.All:
                blue.OnRequestAction(draftController, CurrentPhase);
                red.OnRequestAction(draftController, CurrentPhase);
                break;
        }
    }

    GameFlowData currentFlow;
    public Team CurrentTurn => currentFlow.Turn;
    public GamePhase CurrentPhase => currentFlow.Phase;

    public void GameStart() => ProgressGame();
}
