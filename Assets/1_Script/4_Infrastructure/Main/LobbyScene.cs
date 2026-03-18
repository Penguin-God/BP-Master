using Match;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] LeagueScheduleSO scheduleSO;
    [SerializeField] MoveGame moveGame;
    void Awake()
    {
        var playerDatas = new PlayerMatchData(new PlayerData(1, "@@", new JsonMasterySaver().Load().BoardCollection), new PlayerData(2, "AI", new JsonMasterySaver().Load().BoardCollection));
        MatchContext.MatchInit(playerDatas, 2, ChampionDataLoder.AllId);
        var leagueScheduleUsecase = new LeagueScheduleUsecase(scheduleSO.CreateFlow(), 1, new PlayerPrefsScheduleStorage(), new BattleInintialzer(), null);
        moveGame.Inject(leagueScheduleUsecase);
    }
}
