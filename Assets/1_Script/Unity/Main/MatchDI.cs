using System.Linq;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    SlotStorageManager slotManager;
    [SerializeField] ChampionRepository champManager;
    GameBanPickStorage storage;

    PhaseManager phaseManager;
    PhaseEventDispatcher phaseEventDispatcher = new PhaseEventDispatcher();
    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] AI_Main ai_main;
    Team playerTeam;
    [SerializeField] UtilKey utilKey;

    SlotStorage<Champion> championSlots = new();
    SlotStorage<ChampionStatus> statusSlots = new();

    public void GameStart(Team playerTeam)
    {
        this.playerTeam = playerTeam;
        masteryGenerator.CreateRandomRoster(matchConfig.TeamSize);

        ai_main.Init(EnumCaster.GetOppoentTeam(playerTeam), phaseEventDispatcher);
        storage = new GameBanPickStorage(champManager.AllId);

        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher);
        utilKey.Init(storage, phaseManager);

        // phaseEventDispatcher.OnPhaseSkill += Trait;
        phaseEventDispatcher.OnPhaseDone += OnDone;
        storage.OnPick += OnPick;

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, statusSlots); // start보다 먼저

        ai_main.InitAI_BanPick(phaseManager, storage);

        phaseManager.Start();
    }

    void OnPick(SlotData slotData, int id)
    {
        var champion = champManager.GetChampionData(id).CreateChampion();
        championSlots.AddSlot(slotData.Team, champion);
        statusSlots.AddSlot(slotData.Team, champion.Status);
        new TraitFactory(matchConfig.TraitConfig, statusSlots).Create(slotData.Team, champion.Status.TraitType).Do();
    }

    bool initTrait;
    void Trait(Team team)
    {
        if (initTrait) return;
        initTrait = true;
        slotManager = new SlotStorageManager(storage, champManager);

        var skillController = new SkillUseController(slotManager.StatusSlots);
        skillController.OnUseSkill += slot => slotManager.SkillUseFlagSlot.ChangeSlot(slot, true);
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        var filter = new SkillSlotFilter(slotManager.SkillUseFlagSlot);

        matchUI_Controller.SkillUI_Init(playerTeam, phaseEventDispatcher, skillController, slotManager, filter);

        var traitFactory = new TraitFactory(matchConfig.TraitConfig, slotManager.StatusSlots);
        var traitExecutor = new TraitExecutor(traitFactory);
        traitExecutor.ExecuteAllTriat(slotManager.StatusSlots);

        ApplyMastery(); // 마지막에
        ai_main.InitAI_Trait(filter, slotManager, skillController, matchConfig.TeamSize);
    }

    void ApplyMastery()
    {
        new TeamMasteryApplier().ApplyMastery(storage.PickIds.GetTeam(Team.Blue).ToArray(), slotManager.StatusSlots.GetTeam(Team.Blue).ToArray(), masteryGenerator.GetTeamMasteries(Team.Blue));
        new TeamMasteryApplier().ApplyMastery(storage.PickIds.GetTeam(Team.Red).ToArray(), slotManager.StatusSlots.GetTeam(Team.Red).ToArray(), masteryGenerator.GetTeamMasteries(Team.Red));
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        slotManager = new SlotStorageManager(storage, champManager);
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(slotManager.StatusSlots);
        matchUI_Controller.Done(result);
    }
}
