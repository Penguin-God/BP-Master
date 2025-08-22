

public class MatchManager
{
    readonly AgentManager agentManager;
    readonly PhaseManager phaseManager;

    public MatchManager(PhaseManager phaseManager, AgentManager agentManager)
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

    public void GameStart()
    {
        phaseManager.GetNextFlow();
        agentManager.ChangePhase(CurrentPhase, CurrentTurn);
    }
}
