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
    [SerializeField] int ai_id;
    readonly Dictionary<Team, int> idByTeam = new();

    public void GameStart(Team playerTeam)
    {
        var storage = MatchContext.Storage;
        ai_id = MatchContext.CurrentMatch.GetOpponentId(playerId);

        idByTeam.Add(playerTeam, playerId);
        idByTeam.Add(EnumCaster.GetOppoentTeam(playerTeam), ai_id); // 이거 문제 많음

        masteryRegistry.InitTeamMastery(playerTeam, MatchContext.ParticipantRepository.Get(Participant.Player).Mastery);
        masteryRegistry.InitTeamMastery(EnumCaster.GetOppoentTeam(playerTeam), MatchContext.ParticipantRepository.Get(Participant.AI).Mastery);

        var phaseEventDispatcher = new PhaseEventDispatcher();
        var phaseAdvancer = new PhaseAdvancer(gamePhaseLoder.LoadPhase());
        PhaseFlowOrchestrator phaseManager = CreatePhaseOrchestrator(phaseAdvancer, phaseEventDispatcher, championSelector, ai_main, playerTeam);

        phaseManager.OnGameEnd += OnDone;

        // 로직 추출하기
        banPickHandler = new BanPickHandler(champManager.GetCatalog(), storage);
        var actionEventDispathcer = new BanPickEventDispatcher();
        banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;
        var skillController = new SkillUsecase(PickSlotFacade.ChampionSlots, new SkillRunner(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher), new SkillCondtionFactory()));
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);

        banPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => phaseManager.SubmitAction(team);

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, skillController, masteryRegistry, banPickHandler); // start보다 먼저

        ai_main.Init(ai_id, EnumCaster.GetOppoentTeam(playerTeam), storage, skillController, champManager.GetCatalog(), masteryRegistry, banPickHandler, phaseAdvancer);

        phaseManager.Start();
    }

    PhaseFlowOrchestrator CreatePhaseOrchestrator(PhaseAdvancer phaseAdvancer, PhaseEventDispatcher phaseEventDispatcher, IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return new(phaseAdvancer, phaseEventDispatcher, new TeamPhaseEntryDispatcher(blue, red));
    }

    void ApplyMastery(Champion champion, Team team)
    {
        var masteryApplier = new MasteryApplier(masteryRegistry.GetTeamMasteryManager(team));
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        var matchEnd = MatchContext.EndMatch(idByTeam[result.Winner]);
        matchUI_Controller.Done(result, matchEnd);
    }
}