using Match;
using UnityEngine;

public class AAAA : IChampionProvider // 데이터 매니저 만들기?
{
    public ChampionProfile GetProfile(int id)
    {
        var so = ChampionDataLoder.GetChampionData(id);
        return new ChampionProfile(id, so.name, so.StatData, so.Skill);
    }
}

public class LobbyScene : MonoBehaviour
{
    [SerializeField] LeagueScheduleSO scheduleSO;
    [SerializeField] MoveGame moveGame;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;

    void Awake()
    {
        var leagueScheduleUsecase = new LeagueScheduleUsecase(scheduleSO.CreateFlow(), 1, new PlayerPrefsScheduleStorage(), new BattleInintialzer(), null);
        moveGame.Inject(leagueScheduleUsecase);

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO));

        var playerDatas = new PlayerMatchData(new PlayerData(1, "@@", inventory.BoardCollection), new PlayerData(2, "AI", inventory.BoardCollection));
        MatchContext.MatchInit(playerDatas, 2, ChampionDataLoder.AllId);
    }
}
