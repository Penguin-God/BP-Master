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
    public ScheduleFlow scheduleFlow;
    public void Init(UI_LeagueSchedule uiSchedule, UI_Leaderboard uiLeaderboard)
    {
        _uiSchedule = uiSchedule;
        _uiLeaderboard = uiLeaderboard;
    }

    LeagueScheduleUsecase CreateLeague()
    {
        var storage = new PlayerPrefsScheduleStorage(StorageKey.LeagueKey);
        scheduleFlow = leagueScheduleSO.CreateFlow(storage.LoadIndex());
        return CreateUsecase(scheduleFlow, StorageKey.LeagueKey);
    }

    // 토너먼트 경기목록 로드하든 가져오든하기
    LeagueScheduleUsecase CreateTournament(ScheduleFlow flow)
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

    public LeagueScheduleUsecase CreateSchedule(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.League: return CreateLeague();
            case MatchType.Tournament: return CreateTournament(null);
            default: throw new System.Exception($"맞는 매치 타입이 없음 : {matchType}");
        }
    }
}

public static class StorageKey
{
    public static readonly string LeagueKey = "League_CurrentIndex";
}