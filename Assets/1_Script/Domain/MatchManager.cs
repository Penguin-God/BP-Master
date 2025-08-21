

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
        agentManager.ChangePhase(CurrentPhase);
    }

    public Team CurrentTurn => phaseManager.CurrentPhaseData.Phase.CurrentTeam; // 이게 맞노. 턴 검사 어디서 함? ㅋㅋ
    public GamePhase CurrentPhase => phaseManager.CurrentPhase;

    public void GameStart()
    {
        agentManager.ChangePhase(CurrentPhase);
    }
}
