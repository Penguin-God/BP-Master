using Match;
using UnityEngine.SceneManagement;

public class BattleInintialzer : IBattleResolver
{
    const string BattleSceneName = "Battle";
    public void Resolve(MatchData match)
    {
        MatchContext.MatchInit(match, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene(BattleSceneName);
    }
}

//public class AI_BattleResolver : IBattleResolver
//{
//    readonly AIFactorySO aiFactory;
//    readonly GamePhaseLoderSO gamePhaseLoderSO;
//    readonly BonusDataFactory bonusDataSO;
//    readonly AIPlayerDataCatalogSO aiDataCatalog;
//    readonly MasteryRegistryFactorySO masteryFactorySO;

//    public AI_BattleResolver(AIFactorySO aiFactory,GamePhaseLoderSO gamePhaseLoderSO, BonusDataFactory bonusDataSO, AIPlayerDataCatalogSO aiDataCatalog, MasteryRegistryFactorySO masteryFactorySO)
//    {
//        this.aiFactory = aiFactory;
//        this.gamePhaseLoderSO = gamePhaseLoderSO;
//        this.bonusDataSO = bonusDataSO;
//        this.aiDataCatalog = aiDataCatalog;
//        this.masteryFactorySO = masteryFactorySO;
//    }

//    public void Resolve(MatchData match)
//    {
//        MatchContext.MatchInit(match, 2, ChampionDataLoder.AllId);
//        StartSingleBattle(match);
//    }

//    void StartSingleBattle(MatchData match)
//    {
//        var storage = MatchContext.Storage;
//        var phaseAdvancer = new PhaseAdvancer(gamePhaseLoderSO.LoadPhase());
//        var catalog = ChampionDataLoder.GetCatalog();

//        var blueBoard = aiDataCatalog.LoadPlayer(match.Id1).MasteryBoardCollection;
//        var redBoard = aiDataCatalog.LoadPlayer(match.Id2).MasteryBoardCollection;
//        var registry = masteryFactorySO.CreateRegistry(blueBoard, redBoard);

//        var core = new MatchCore(catalog, storage, phaseAdvancer, registry);

//        var blueEntry = CreateEntry(Team.Blue, match.Id1, core, storage, catalog, phaseAdvancer);
//        var redEntry = CreateEntry(Team.Red, match.Id2, core, storage, catalog, phaseAdvancer);

//        core.SetupPhaseManager(blueEntry, redEntry);

//        // 이벤트가 발생하면 게임 종료 처리를 수행하는 지역 함수
//        void OnDone()
//        {
//            core.PhaseManager.OnGameEnd -= OnDone;
//            HandleGameEnd(core.BanPickHandler.PickSlotFacade, match);
//        }

//        core.PhaseManager.OnGameEnd += OnDone;
//        core.PhaseManager.Start();
//    }

//    void HandleGameEnd(PickSlotFacade pickSlotFacade, MatchData match)
//    {
//        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
//        MatchResult result = new MatchResultConverter(builder).ToResult(pickSlotFacade.StatusSlots);

//        // 무승부일 경우 해당 라운드 재경기
//        if (result.Winner == Team.All)
//        {
//            StartSingleBattle(match);
//            return;
//        }

//        // 승자 판별
//        int winnerId = result.Winner == Team.Blue ? match.Id1 : match.Id2;

//        // MatchContext.EndMatch가 true를 반환하면 최종 승리자 확정 (예: 2승 달성)
//        if (!MatchContext.EndMatch(winnerId))
//        {
//            // 아직 최종 승리자가 안 나왔으므로 다음 세트 진행
//            StartSingleBattle(match);
//        }
//        // 최종 종료 시 재귀가 끝나며 자연스럽게 반환(Return)됩니다.
//    }

//    AI_Entry CreateEntry(Team team, int id, MatchCore core, BanPickStorage storage, ChampionCatalog catalog, PhaseAdvancer phaseAdvancer) => new AI_Entry(team, id, aiFactory, storage, core.SkillController, catalog, core.MasteryRegistry, core.BanPickHandler, phaseAdvancer);
//}
