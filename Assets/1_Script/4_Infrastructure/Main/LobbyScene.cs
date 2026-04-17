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

public class LocalPlayerDataLoader : IPlayerDataLoader
{
    readonly string playerName;
    readonly JsonMasterySaver saver;

    public LocalPlayerDataLoader(string playerName, JsonMasterySaver saver)
    {
        this.playerName = playerName;
        this.saver = saver;
    }

    public PlayerData LoadPlayer(int id)
    {
        var inventory = saver.Load();
        if(inventory == null)
            inventory = new MasteryProfile(0);
        return new PlayerData(id, playerName, inventory.BoardCollection);
    }
}

public class LobbyScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] MoveGame moveGame;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] UI_LeagueSchedule uI_LeagueSchedule;
    [SerializeField] UI_Leaderboard uI_Leaderboard;
    [SerializeField] TutorialTrigger tutorialTrigger;
    [SerializeField] PlayerDataProviderFactorySO playerDataProviderFactory;
    [SerializeField] ScheduleFactorySO scheduleFactorySO;

    void Awake()
    {
        
        scheduleFactorySO.Init(uI_LeagueSchedule, uI_Leaderboard);
        var flow = scheduleFlowFactorySO.Create();
        moveGame.Inject(scheduleFactorySO.CreateSchedule(flow));

        MatchContext.OnSeriesFinished -= SaveGameProgress;
        MatchContext.OnSeriesFinished += SaveGameProgress;

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);
        uI_LeagueSchedule.Init(new SchedulePaginationPresenter(flow, matchConfigSO.UserId));

        uI_Leaderboard.Init(new LeaderboardPresenter(new PlayerPrefsLeagueRecordStorage(), playerDataProviderFactory.CreatePlayerDataProvider()));
        tutorialTrigger.PlayTutorial();
    }

    [SerializeField] ScheduleFlowFactorySO scheduleFlowFactorySO;
    void SaveGameProgress(MatchData matchData, MatchWinCounter winCounter)
    {
        int index = scheduleFlowFactorySO.CreateStorage().LoadIndex() + 1;
        scheduleFlowFactorySO.CreateStorage().SaveIndex(index);

        int p1Wins = winCounter.GetWin(matchData.Id1);
        int p2Wins = winCounter.GetWin(matchData.Id2);
        var recordUsecase = new LeagueRecordUseCase(new PlayerPrefsLeagueRecordStorage());
        recordUsecase.RecordMatch(matchData.Id1, p1Wins, matchData.Id2, p2Wins);
    }
}
