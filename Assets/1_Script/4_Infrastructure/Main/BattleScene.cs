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
    Dictionary<Team, int> playerDatas = new();

    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;

    
    public void GameStart(Team playerTeam)
    {
        int ai_id = MatchContext.CurrentMatch.GetOpponentId(matchCoreFactorySO.UserId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerDatas.Add(playerTeam, matchCoreFactorySO.UserId);
        playerDatas.Add(aiTeam, ai_id);

        var championCatalog = ChampionDataLoder.GetCatalog();
        var storage = MatchContext.Storage;
        var core = matchCoreFactorySO.CreateMatchCore(storage, championCatalog, playerDatas);

        var masteryRegistry = core.MasteryRegistry;

        var (blue, red) = CreatePhaseOrchestrator(championSelector, ai_main, playerTeam);
        core.SetupPhaseManager(blue, red);
        var phaseManager = core.PhaseManager;
        var banPickHandler = core.BanPickHandler;
        PickSlotFacade = banPickHandler.PickSlotFacade;

        matchUI_Controller.Init(playerTeam, storage, phaseManager, core.PhaseEventDispatcher, core.SkillController, masteryRegistry, banPickHandler); // start보다 먼저

        ai_main.Init(ai_id, aiTeam, storage, core.SkillController, championCatalog, masteryRegistry, banPickHandler, core.PhaseAdvancer);

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
        var matchEnd = MatchContext.EndMatch(playerDatas[result.Winner]);
        matchUI_Controller.Done(result, matchEnd);
    }
}