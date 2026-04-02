using Match;
using UnityEngine.SceneManagement;

public class BattleInitializer : IBattleResolver
{
    const string BattleSceneName = "Battle";

    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}
public class AI_BattleResolver : IBattleResolver
{
    readonly AIBattleSimulatorSO _simulator;
    readonly UI_LeagueSchedule uI_LeagueSchedule;

    public AI_BattleResolver(AIBattleSimulatorSO simulator, UI_LeagueSchedule uI_LeagueSchedule)
    {
        _simulator = simulator;
        this.uI_LeagueSchedule = uI_LeagueSchedule;
    }
    public void Resolve(MatchData match) => _simulator.SimulateMatch(match, 2, OnGameDone);

    void OnGameDone(MatchResult result)
    {
        uI_LeagueSchedule.RefreshUI();
    }
}