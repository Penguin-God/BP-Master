using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleFactorySO", menuName = "BP Master/ScheduleFactorySO")]
public class ScheduleFactorySO : ScriptableObject
{
    [SerializeField] AIBattleSimulatorSO aIBattleSimulatorSO;
    [SerializeField] MatchConfigSO matchConfigSO;

    const string TournamentKey = "Tournament_CurrentIndex";

    UI_LeagueSchedule _uiSchedule;
    UI_Leaderboard _uiLeaderboard;
    public void Init(UI_LeagueSchedule uiSchedule, UI_Leaderboard uiLeaderboard)
    {
        _uiSchedule = uiSchedule;
        _uiLeaderboard = uiLeaderboard;
    }

    public LeagueScheduleUsecase CreateLeagueUsecase(ScheduleFlow flow)
    {
        var storage = new PlayerPrefsScheduleStorage(StorageKey.LeagueKey);
        return CreateUsecase(flow, storage);
    }

    public LeagueScheduleUsecase CreateTournamentUsecase(ScheduleFlow flow)
    {
        var storage = new PlayerPrefsScheduleStorage(TournamentKey);
        return CreateUsecase(flow, storage);
    }

    LeagueScheduleUsecase CreateUsecase(ScheduleFlow flow, IScheduleStorage storage)
    {
        var aiResolver = new AI_BattleResolver(aIBattleSimulatorSO, _uiSchedule, _uiLeaderboard);
        var userResolver = new BattleInitializer(matchConfigSO.TargetWinCount);

        return new LeagueScheduleUsecase(flow, matchConfigSO.UserId, storage, userResolver, aiResolver);
    }
}

public static class StorageKey
{
    public static readonly string LeagueKey = "League_CurrentIndex";
}