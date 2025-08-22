
public class MatchManager
{
    readonly DraftActionController draftController;
    readonly PhaseManager phaseManager;

    IActionHandler blue;
    IActionHandler red;
    public MatchManager(PhaseManager phaseManager, DraftActionController agentManager, IActionHandler blue, IActionHandler red)
    {
        this.phaseManager = phaseManager;
        this.draftController = agentManager;
        this.blue = blue;
        this.red = red;

        this.draftController.OnActionDone += ProgressGame;
    }

    void ProgressGame()
    {
        currentFlow = phaseManager.GetNextFlow();
        draftController.ChangePhase(CurrentPhase, CurrentTurn);
        switch (CurrentTurn)
        {
            case Team.Blue: blue.OnRequestAction(draftController, CurrentPhase); break;
            case Team.Red: red.OnRequestAction(draftController, CurrentPhase); break;
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
