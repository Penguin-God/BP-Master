using UnityEngine;
using Match;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;

    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] AI_Main ai_main;
    [SerializeField] GamePhaseLoderSO gamePhaseLoder;

    PickSlotFacade PickSlotFacade => banPickHandler.PickSlotFacade;
    [SerializeField] ChampionSelector_UI championSelector;
    MasteryRegistry masteryRegistry = new();
    BanPickHandler banPickHandler;

    [SerializeField] int playerId = 1;

    readonly Dictionary<Team, PlayerData> playerDatas = new();

    public void GameStart(Team playerTeam)
    {
        var storage = MatchContext.Storage;

        int ai_id = MatchContext.CurrentMatch.GetOpponentId(playerId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerDatas.Add(playerTeam, MatchContext.GetPlayerData(playerId));
        playerDatas.Add(aiTeam, MatchContext.PlayerMatchData.GetPlayer(ai_id));

        var masteryFactory = new MasteryStatCollectionFactory(new MasteryMultiplier(15, 15, 1));
        masteryRegistry.InitTeamMastery(playerTeam, masteryFactory.Create(playerDatas[playerTeam].MasteryBoardCollection));
        masteryRegistry.InitTeamMastery(aiTeam, masteryFactory.Create(playerDatas[aiTeam].MasteryBoardCollection));

        var phaseEventDispatcher = new PhaseEventDispatcher();
        var phaseAdvancer = new PhaseAdvancer(gamePhaseLoder.LoadPhase());
        PhaseFlowOrchestrator phaseManager = CreatePhaseOrchestrator(phaseAdvancer, phaseEventDispatcher, championSelector, ai_main, playerTeam);

        phaseManager.OnGameEnd += OnDone;

        banPickHandler = new BanPickHandler(champManager.GetCatalog(), storage);
        var actionEventDispathcer = new BanPickEventDispatcher();
        banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;
        var skillController = new SkillUsecase(PickSlotFacade.ChampionSlots, new SkillRunner(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher), new SkillCondtionFactory()));
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);

        banPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => phaseManager.SubmitAction(team);

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, skillController, masteryRegistry, banPickHandler); // start보다 먼저

        ai_main.Init(ai_id, aiTeam , storage, skillController, champManager.GetCatalog(), masteryRegistry, banPickHandler, phaseAdvancer);

        phaseManager.Start();
    }

    PhaseFlowOrchestrator CreatePhaseOrchestrator(PhaseAdvancer phaseAdvancer, PhaseEventDispatcher phaseEventDispatcher, IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return new(phaseAdvancer, phaseEventDispatcher, new TeamPhaseEntryDispatcher(blue, red));
    }

    void ApplyMastery(Champion champion, Team team) => new MasteryApplier(masteryRegistry.GetTeamMasteryCollection(team)).ApplyMastery(champion.Id, champion.Status);

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        var matchEnd = MatchContext.EndMatch(playerDatas[result.Winner].Id);
        matchUI_Controller.Done(result, matchEnd);
    }
}