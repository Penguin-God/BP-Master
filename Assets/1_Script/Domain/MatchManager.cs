

public class MatchManager
{
    readonly DraftActionController agentManager;
    readonly PhaseManager phaseManager;

    public MatchManager(PhaseManager phaseManager, DraftActionController agentManager)
    {
        this.phaseManager = phaseManager;
        this.agentManager = agentManager;

        this.agentManager.OnActionDone += ProgressGame;
    }

    void ProgressGame()
    {
        currentFlow = phaseManager.GetNextFlow();
        agentManager.ChangePhase(CurrentPhase, CurrentTurn);
    }

    GameFlowData currentFlow;
    public Team CurrentTurn => currentFlow.Turn;
    public GamePhase CurrentPhase => currentFlow.Phase;

    public void GameStart() => ProgressGame();
}
