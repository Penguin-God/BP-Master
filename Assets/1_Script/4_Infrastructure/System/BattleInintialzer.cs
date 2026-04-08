using Match;
using UnityEngine.SceneManagement;

public class BattleInitializer : IBattleResolver
{
    const string BattleSceneName = "Battle";
    readonly int WinCount;
    public BattleInitializer(int winCount) => WinCount = winCount;

    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, WinCount, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}

public class AI_BattleResolver : IBattleResolver
{
    readonly AIBattleSimulatorSO _simulator;
    readonly UI_LeagueSchedule uI_LeagueSchedule;
    readonly UI_Leaderboard uI_Leaderboard;

    public AI_BattleResolver(AIBattleSimulatorSO simulator, UI_LeagueSchedule uI_LeagueSchedule, UI_Leaderboard uI_Leaderboard)
    {
        _simulator = simulator;
        this.uI_LeagueSchedule = uI_LeagueSchedule;
        this.uI_Leaderboard = uI_Leaderboard;
    }
    public void Resolve(MatchData match) => _simulator.SimulateMatch(match, 2, null, OnGameDone);

    void OnGameDone()
    {
        uI_LeagueSchedule.RefreshUI();
        uI_Leaderboard.RefreshUI();
    }
}