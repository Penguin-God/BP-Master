using UnityEngine;
using Match;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour
{
    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] AI_Main ai_main;

    PickSlotFacade PickSlotFacade;
    [SerializeField] ChampionSelector_UI championSelector;

    [SerializeField] GamePhaseLoderSO gamePhaseLoder;
    [SerializeField] MasteryRegistryFactorySO masteryFactorySO;
    [SerializeField] int playerId = 1;

    readonly Dictionary<Team, PlayerData> playerDatas = new();

    //public void GameStart(Team playerTeam)
    //{
    //    var storage = MatchContext.Storage;

    //    int ai_id = MatchContext.CurrentMatch.GetOpponentId(playerId);
    //    Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

    //    playerDatas.Add(playerTeam, MatchContext.GetPlayerData(playerId));
    //    playerDatas.Add(aiTeam, MatchContext.PlayerMatchData.GetPlayer(ai_id));

    //    var masteryFactory = new MasteryStatCollectionFactory(new MasteryMultiplier(15, 15, 1));
    //    masteryRegistry.InitTeamMastery(playerTeam, masteryFactory.Create(playerDatas[playerTeam].MasteryBoardCollection));
    //    masteryRegistry.InitTeamMastery(aiTeam, masteryFactory.Create(playerDatas[aiTeam].MasteryBoardCollection));

    //    var phaseEventDispatcher = new PhaseEventDispatcher();
    //    var phaseAdvancer = new PhaseAdvancer(gamePhaseLoder.LoadPhase());
    //    PhaseFlowOrchestrator phaseManager = CreatePhaseOrchestrator(phaseAdvancer, phaseEventDispatcher, championSelector, ai_main, playerTeam);

    //    phaseManager.OnGameEnd += OnDone;

    //    banPickHandler = new BanPickHandler(champManager.GetCatalog(), storage);
    //    var actionEventDispathcer = new BanPickEventDispatcher();
    //    banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;
    //    var skillController = new SkillUsecase(PickSlotFacade.ChampionSlots, new SkillRunner(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher), new SkillCondtionFactory()));
    //    skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);

    //    banPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => phaseManager.SubmitAction(team);

    //    matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, skillController, masteryRegistry, banPickHandler); // start보다 먼저

    //    ai_main.Init(ai_id, aiTeam , storage, skillController, champManager.GetCatalog(), masteryRegistry, banPickHandler, phaseAdvancer);

    //    phaseManager.Start();
    //}

    [SerializeField] AIPlayerDataCatalogSO aiPlayerDataCatalog;
    [SerializeField] int mainPlayerId = 1;
    [SerializeField] string mainPlayerName = "@@";

    public void GameStart(Team playerTeam)
    {
        var dataIO = new JsonMasterySaver();

        // IPlayerDataLoader를 구현한 구체 클래스들을 주입합니다.
        IPlayerDataLoader localLoader = new LocalPlayerDataLoader(mainPlayerName, dataIO);
        IPlayerDataLoader aiLoader = aiPlayerDataCatalog;

        var dataProvider = new PlayerDataProvider(mainPlayerId, localLoader, aiLoader);

        int ai_id = MatchContext.CurrentMatch.GetOpponentId(mainPlayerId);

        var playerDatass = new PlayerMatchData(dataProvider.GetPlayer(mainPlayerId), dataProvider.GetPlayer(ai_id));

        // =======================================================

        var storage = MatchContext.Storage;
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerDatas.Add(playerTeam, MatchContext.GetPlayerData(playerId));
        playerDatas.Add(aiTeam, MatchContext.PlayerMatchData.GetPlayer(ai_id));

        var phaseAdvancer = gamePhaseLoder.CreateAdvacer();
        var championCatalog = ChampionDataLoder.GetCatalog();
        var core = new MatchCore(championCatalog, storage, phaseAdvancer, masteryFactorySO.CreateRegistry(playerDatas));

        var masteryRegistry = core.MasteryRegistry;

        var (blue, red) = CreatePhaseOrchestrator(championSelector, ai_main, playerTeam);
        core.SetupPhaseManager(blue, red);
        var phaseManager = core.PhaseManager;
        var banPickHandler = core.BanPickHandler;
        PickSlotFacade = banPickHandler.PickSlotFacade;

        matchUI_Controller.Init(playerTeam, storage, phaseManager, core.PhaseEventDispatcher, core.SkillController, masteryRegistry, banPickHandler); // start보다 먼저

        ai_main.Init(ai_id, aiTeam, storage, core.SkillController, championCatalog, masteryRegistry, banPickHandler, phaseAdvancer);

        phaseManager.OnGameEnd += OnDone;
        phaseManager.Start();
    }

    (IPhaseEntry blue, IPhaseEntry red) CreatePhaseOrchestrator(IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return (blue, red);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        var matchEnd = MatchContext.EndMatch(playerDatas[result.Winner].Id);
        matchUI_Controller.Done(result, matchEnd);
    }
}