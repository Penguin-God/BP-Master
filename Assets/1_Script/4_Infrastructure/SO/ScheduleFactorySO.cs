using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleFactorySO", menuName = "BP Master/ScheduleFactorySO")]
public class ScheduleFactorySO : ScriptableObject
{
    [SerializeField] AIBattleSimulatorSO aIBattleSimulatorSO;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] LeagueScheduleSO leagueScheduleSO;

    const string TournamentKey = "Tournament_CurrentIndex";

    UI_LeagueSchedule _uiSchedule;
    UI_Leaderboard _uiLeaderboard;
    public void Init(UI_LeagueSchedule uiSchedule, UI_Leaderboard uiLeaderboard)
    {
        _uiSchedule = uiSchedule;
        _uiLeaderboard = uiLeaderboard;
    }

    LeagueScheduleUsecase CreateLeagueUsecase()
    {
        var storage = new PlayerPrefsScheduleStorage(StorageKey.LeagueKey);
        return CreateUsecase(leagueScheduleSO.CreateFlow(storage.LoadIndex()), StorageKey.LeagueKey);
    }

    // 토너먼트 경기목록 로드하든 가져오든하기
    LeagueScheduleUsecase CreateTournamentUsecase(ScheduleFlow flow)
    {
        return CreateUsecase(flow, TournamentKey);
    }

    LeagueScheduleUsecase CreateUsecase(ScheduleFlow flow, string key)
    {
        var storage = new PlayerPrefsScheduleStorage(key);
        var aiResolver = new AI_BattleResolver(aIBattleSimulatorSO, _uiSchedule, _uiLeaderboard);
        var userResolver = new BattleInitializer(matchConfigSO.TargetWinCount);

        return new LeagueScheduleUsecase(flow, matchConfigSO.UserId, storage, userResolver, aiResolver);
    }

    // 스위치 문
    public LeagueScheduleUsecase CreateSchedule(MatchType matchType)
    {
        return null;
    }
}

public static class StorageKey
{
    public static readonly string LeagueKey = "League_CurrentIndex";
}