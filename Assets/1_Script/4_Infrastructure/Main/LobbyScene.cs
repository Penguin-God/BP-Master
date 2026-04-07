using Match;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] AIPlayerDataCatalogSO aiPlayerDataCatalog;
    [SerializeField] TutorialTrigger tutorialTrigger;

    LeagueRecordUseCase recordUsecase;
    void Awake()
    {
        recordUsecase = new LeagueRecordUseCase(new PlayerPrefsLeagueRecordStorage());

        var scheduleStorage = new PlayerPrefsScheduleStorage();
        var matchFlow = scheduleSO.CreateFlow(scheduleStorage.LoadIndex());
        var leagueScheduleUsecase = new LeagueScheduleUsecase(matchFlow, matchConfigSO.UserId, scheduleStorage, new BattleInitializer(matchConfigSO.TargetWinCount), new AI_BattleResolver(aIBattleSimulatorSO, uI_LeagueSchedule));
        moveGame.Inject(leagueScheduleUsecase);

        MatchContext.OnSeriesFinished -= HandleSeriesFinished;
        MatchContext.OnSeriesFinished += HandleSeriesFinished;

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);
        uI_LeagueSchedule.Init(new SchedulePaginationPresenter(matchFlow, matchConfigSO.UserId));

        IPlayerDataLoader localLoader = new LocalPlayerDataLoader("펭귄갓", new JsonMasterySaver());
        var dataProvider = new PlayerDataProvider(1, localLoader, aiPlayerDataCatalog);
        uI_Leaderboard.Init(new LeaderboardPresenter(new PlayerPrefsLeagueRecordStorage().LoadAll(), dataProvider));
        // 컬랙션이 아니라 PlayerPrefsLeagueRecordStorage자체를 줘야 함
        // PlayerDataProvider만드는 factory SO 필요
        // 게임 끝날때마다 UI갱신

        tutorialTrigger.PlayTutorial();
    }

    void HandleSeriesFinished(MatchData matchData, MatchWinCounter winCounter)
    {
        int p1Wins = winCounter.GetWin(matchData.Id1);
        int p2Wins = winCounter.GetWin(matchData.Id2);

        var records = recordUsecase.RecordMatch(matchData.Id1, p1Wins, matchData.Id2, p2Wins);
        PrintLeagueStandings(records.GetAll());
    }

    void PrintLeagueStandings(IEnumerable<LeagueRecord> records)
    {
        var logLines = records
            .OrderByDescending(x => x.Win)
            .Select(kvp => $"ID {kvp.Id} : {kvp.Win}승 {kvp.Lose}패 (승점: {kvp.Score})");

        Debug.Log("=== 현재 리그 순위 ===\n" + string.Join("\n", logLines));
    }
}
