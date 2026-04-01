using Match;
using System.Collections.Generic;
using System.Linq;
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

    public AI_BattleResolver(AIBattleSimulatorSO simulator) => _simulator = simulator;
    public void Resolve(MatchData match) => _simulator.SimulateMatch(match, 2, OnGameDone);

    void OnGameDone(MatchResult result)
    {
        UnityEngine.Debug.Log($"승자 : {result.Winner}");
    }
}