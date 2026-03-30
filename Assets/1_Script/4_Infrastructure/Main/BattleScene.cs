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
    Dictionary<Team, PlayerData> playerDatas = new();


    [SerializeField] AIPlayerDataCatalogSO aiPlayerDataCatalog;
    [SerializeField] int userId = 1;
    [SerializeField] string mainPlayerName = "@@";

    public void GameStart(Team playerTeam)
    {
        IPlayerDataLoader localLoader = new LocalPlayerDataLoader(mainPlayerName, new JsonMasterySaver());

        var dataProvider = new PlayerDataProvider(userId, localLoader, aiPlayerDataCatalog);

        int ai_id = MatchContext.CurrentMatch.GetOpponentId(userId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerDatas.Add(playerTeam, dataProvider.GetPlayer(userId));
        playerDatas.Add(aiTeam, dataProvider.GetPlayer(ai_id));

        var phaseAdvancer = gamePhaseLoder.CreateAdvacer();
        var championCatalog = ChampionDataLoder.GetCatalog();
        var storage = MatchContext.Storage;
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