using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionRepository champManager;

    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] AI_Main ai_main;

    PickSlotFacade PickSlotFacade => pickHandler.PickSlotFacade;
    [SerializeField] ChampionSelector_UI championSelector;
    MasteryRegistry masteryRegistry = new();
    PickHandler pickHandler;
    MatchFlowUsecase matchFlowUsecase;
    MatchRecord matchRecord;
    MatchManager matchManager;
    public void GameStart(Team playerTeam)
    {
        matchManager = FindAnyObjectByType<MatchManager>();
        matchRecord = matchManager.Record;
        matchFlowUsecase = new MatchFlowUsecase(matchRecord, playerTeam);
        var storage = matchManager.Storage;

        masteryRegistry.InitTeamMastery(playerTeam, matchManager.participantRepository.Get(Participant.Player).Mastery);
        masteryRegistry.InitTeamMastery(EnumCaster.GetOppoentTeam(playerTeam), matchManager.participantRepository.Get(Participant.AI).Mastery);

        var phaseEventDispatcher = new PhaseEventDispatcher();
        PhaseFlowOrchestrator phaseManager = CreatePhaseOrchestrator(phaseEventDispatcher, championSelector, ai_main, playerTeam);
        phaseManager.OnGameEnd += OnDone;
        phaseManager.OnGameEnd += matchManager.EndMatch;

        // 로직 추출하기
        var actionEventDispathcer = new PhaseActionEventDispatcher();
        pickHandler = new PickHandler(champManager.GetCatalog(), actionEventDispathcer);
        storage.OnPick += OnPick;
        var skillController = new SkillUsecase(PickSlotFacade.StatusSlots, new SkillRunner(new SkillExecutorFactory(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher))));
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        storage.OnBan += (team, id) => phaseManager.SubmitAction(team);

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, PickSlotFacade.StatusSlots, PickSlotFacade.SkillSlots, skillController, masteryRegistry); // start보다 먼저

        ai_main.Init(EnumCaster.GetOppoentTeam(playerTeam), storage, PickSlotFacade.SkillSlots, skillController, PickSlotFacade.StatusSlots, champManager.GetCatalog(), masteryRegistry);

        phaseManager.Start();
    }

    PhaseFlowOrchestrator CreatePhaseOrchestrator(PhaseEventDispatcher phaseEventDispatcher, IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher, new TeamPhaseEntryDispatcher(blue, red));
    }

    void OnPick(SlotData slotData, int id)
    {
        PickEffectApplier pickEffectApplier = new PickEffectApplier(masteryRegistry.GetTeamMasteryManager(slotData.Team));
        pickHandler.Pick(slotData.Team, id);
        pickEffectApplier.Apply(slotData.Team, PickSlotFacade.ChampionSlots.GetSlot(slotData));
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        matchFlowUsecase.EndMatch(result.Winner);
        matchUI_Controller.Done(result, matchRecord.IsMatchFinished);
    }
}
