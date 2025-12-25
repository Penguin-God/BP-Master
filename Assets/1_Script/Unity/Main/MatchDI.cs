using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionRepository champManager;

    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] AI_Main ai_main;

    PickSlotFacade PickSlotFacade => pickHandler.PickSlotFacade;
    [SerializeField] ChampionSelector_UI championSelector;
    PickHandler pickHandler;

    public void GameStart(Team playerTeam)
    {
        masteryGenerator.SettingRandomMastery(matchConfig.TeamSize);
        var storage = new GameBanPickStorage(champManager.AllId);

        var phaseEventDispatcher = new PhaseEventDispatcher();
        PhaseFlowOrchestrator phaseManager = CreatePhaseOrchestrator(phaseEventDispatcher, championSelector, ai_main, playerTeam);

        // 로직 추출하기
        pickHandler = new PickHandler(champManager.GetCatalog(), null);
        phaseEventDispatcher.OnPhaseDone += OnDone;
        storage.OnPick += OnPick;
        var skillController = new SkillUseController(PickSlotFacade.StatusSlots);
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        storage.OnBan += (team, id) => phaseManager.SubmitAction(team);

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, PickSlotFacade.StatusSlots, PickSlotFacade.SkillSlots, skillController); // start보다 먼저

        ai_main.Init(EnumCaster.GetOppoentTeam(playerTeam), storage, PickSlotFacade.SkillSlots, skillController);

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
        var traitFactory = new TraitFactory(matchConfig.TraitConfig, PickSlotFacade.StatusSlots);
        PickEffectApplier pickEffectApplier = new PickEffectApplier(traitFactory, masteryGenerator.GetTeamMasteryManager(slotData.Team));
        pickHandler.Pick(slotData.Team, id);
        pickEffectApplier.Apply(slotData.Team, PickSlotFacade.ChampionSlots.GetSlot(slotData));
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        matchUI_Controller.Done(result);
    }
}
