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
        return new PlayerData(id, playerName, inventory.BoardCollection);
    }
}

public class LobbyScene : MonoBehaviour
{
    [SerializeField] LeagueScheduleSO scheduleSO;
    [SerializeField] MoveGame moveGame;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] AIBattleSimulatorSO aIBattleSimulatorSO;

    LeagueRecordUseCase recordUsecase;
    void Awake()
    {
        recordUsecase = new LeagueRecordUseCase(new PlayerPrefsLeagueRecordStorage());

        var leagueScheduleUsecase = new LeagueScheduleUsecase(scheduleSO.CreateFlow(), 1, new PlayerPrefsScheduleStorage(), new BattleInitializer(), new AI_BattleResolver(aIBattleSimulatorSO));
        moveGame.Inject(leagueScheduleUsecase);

        MatchContext.OnSeriesFinished -= HandleSeriesFinished;
        MatchContext.OnSeriesFinished += HandleSeriesFinished;

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO));
    }

    void HandleSeriesFinished(MatchData matchData, MatchWinCounter winCounter)
    {
        int p1Wins = winCounter.GetWin(matchData.Id1);
        int p2Wins = winCounter.GetWin(matchData.Id2);

        var records = recordUsecase.RecordMatch(matchData.Id1, p1Wins, matchData.Id2, p2Wins);
        PrintLeagueStandings(records);
    }

    void PrintLeagueStandings(Dictionary<int, LeagueRecord> records)
    {
        var logLines = records
            .OrderByDescending(x => x.Value.Score)
            .Select(kvp => $"ID {kvp.Key} : {kvp.Value.Win}승 {kvp.Value.Lose}패 (승점: {kvp.Value.Score})");

        Debug.Log("=== 현재 리그 순위 ===\n" + string.Join("\n", logLines));
    }
}
