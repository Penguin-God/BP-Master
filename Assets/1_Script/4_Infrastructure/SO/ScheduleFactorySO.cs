using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleFactorySO", menuName = "BP Master/ScheduleFactorySO")]
public class ScheduleFactorySO : ScriptableObject
{
    [SerializeField] AIBattleSimulatorSO aIBattleSimulatorSO;
    [SerializeField] MatchConfigSO matchConfigSO;

    UI_LeagueSchedule _uiSchedule;
    UI_Leaderboard _uiLeaderboard;
    public void Init(UI_LeagueSchedule uiSchedule, UI_Leaderboard uiLeaderboard)
    {
        _uiSchedule = uiSchedule;
        _uiLeaderboard = uiLeaderboard;
    }

    LeagueScheduleUsecase CreateUsecase(ScheduleFlow flow, string key)
    {
        var aiResolver = new AI_BattleResolver(aIBattleSimulatorSO, _uiSchedule, _uiLeaderboard);
        var userResolver = new BattleInitializer(matchConfigSO.TargetWinCount);

        return new LeagueScheduleUsecase(flow, matchConfigSO.UserId, userResolver, aiResolver);
    }

    public LeagueScheduleUsecase CreateSchedule(ScheduleFlow scheduleFlow)
    {
        return CreateUsecase(scheduleFlow, StorageKey.LeagueKey);
    }
}

public static class StorageKey
{
    public static readonly string LeagueKey = "League_CurrentIndex";
}