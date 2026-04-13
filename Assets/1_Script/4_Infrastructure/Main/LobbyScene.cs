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
    [SerializeField] LeagueScheduleSO scheduleSO;
    [SerializeField] MoveGame moveGame;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] AIBattleSimulatorSO aIBattleSimulatorSO;
    [SerializeField] UI_LeagueSchedule uI_LeagueSchedule;
    [SerializeField] UI_Leaderboard uI_Leaderboard;
    [SerializeField] TutorialTrigger tutorialTrigger;
    [SerializeField] PlayerDataProviderFactorySO playerDataProviderFactory;
    [SerializeField] ScheduleFactorySO scheduleFactorySO;

    LeagueRecordUseCase recordUsecase;
    MatchType matchType;
    void Awake()
    {
        recordUsecase = new LeagueRecordUseCase(new PlayerPrefsLeagueRecordStorage());

        var scheduleStorage = new PlayerPrefsScheduleStorage(StorageKey.LeagueKey);
        var matchFlow = scheduleSO.CreateFlow(scheduleStorage.LoadIndex());

        matchType = matchFlow.IsFinished ? MatchType.Tournament : MatchType.League;

        // var leagueScheduleUsecase = new LeagueScheduleUsecase(matchFlow, matchConfigSO.UserId, scheduleStorage, new BattleInitializer(matchConfigSO.TargetWinCount), new AI_BattleResolver(aIBattleSimulatorSO, uI_LeagueSchedule, uI_Leaderboard));
        scheduleFactorySO.Init(uI_LeagueSchedule, uI_Leaderboard);
        moveGame.Inject(scheduleFactorySO.CreateSchedule(matchType));

        MatchContext.OnSeriesFinished -= HandleSeriesFinished;
        MatchContext.OnSeriesFinished += HandleSeriesFinished;

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);
        uI_LeagueSchedule.Init(new SchedulePaginationPresenter(matchFlow, matchConfigSO.UserId));

        uI_Leaderboard.Init(new LeaderboardPresenter(new PlayerPrefsLeagueRecordStorage(), playerDataProviderFactory.CreatePlayerDataProvider()));
        tutorialTrigger.PlayTutorial();
    }

    void HandleSeriesFinished(MatchData matchData, MatchWinCounter winCounter)
    {
        int p1Wins = winCounter.GetWin(matchData.Id1);
        int p2Wins = winCounter.GetWin(matchData.Id2);
        recordUsecase.RecordMatch(matchData.Id1, p1Wins, matchData.Id2, p2Wins);
    }
}
