using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleFlowFactorySO", menuName = "BP Master/ScheduleFlowFactorySO")]
public class ScheduleFlowFactorySO : ScriptableObject
{
    [SerializeField] LeagueScheduleSO leagueScheduleSO;
    const string TournamentKey = "Tournament_CurrentIndex";
    public ScheduleFlow Create()
    {
        var result = leagueScheduleSO.CreateFlow(CreateStorage().LoadIndex());

        if (result.IsFinished)
        {
            // 여기서 토너먼트 스케줄 채우고 생성
            return null;
        }
        else return result;
    }

    public IScheduleStorage CreateStorage()
    {
        var storage = CreateStorage(StorageKey.LeagueKey);
        if (leagueScheduleSO.CreateFlow(storage.LoadIndex()).IsFinished)
            return CreateStorage(TournamentKey);
        else
            return storage;
    }

    PlayerPrefsScheduleStorage CreateStorage(string key) => new PlayerPrefsScheduleStorage(key);
}
