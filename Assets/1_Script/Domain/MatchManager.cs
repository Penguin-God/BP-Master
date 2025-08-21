

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
        phaseManager.Next();
        agentManager.ChangePhase(CurrentPhase, CurrentTurn);
    }

    public Team CurrentTurn => phaseManager.CurrentPhaseData.Phase.CurrentTeam;
    public GamePhase CurrentPhase => phaseManager.CurrentPhase;

    public void GameStart()
    {
        agentManager.ChangePhase(CurrentPhase, CurrentTurn);
    }
}
