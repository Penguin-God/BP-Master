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
    MatchUI_Controller matchUI_Controller;
    GamerRoster gamerRoster;
    [SerializeField] AI_Main ai_main;
    Team playerTeam;
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        this.playerTeam = playerTeam;
        gamerRoster = GetComponent<GamerRoster>();
        gamerRoster.CreateRandomRoster(matchConfig.TeamSize);


        ai_main.Init(EnumCaster.GetOppoentTeam(playerTeam), phaseEventDispatcher);
        storage = new GameBanPickStorage(champManager.AllId);

        matchUI_Controller = GetComponent<MatchUI_Controller>();

        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher);
        utilKey.Init(storage, phaseManager);

        phaseEventDispatcher.OnPhaseSkill += Trait;
        phaseEventDispatcher.OnPhaseDone += OnDone;

        matchUI_Controller.Init(storage, phaseManager, phaseEventDispatcher); // start보다 먼저

        ai_main.InitAI_BanPick(phaseManager, storage);

        phaseManager.Start();
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

        matchUI_Controller.TraitUI_Init(playerTeam, phaseEventDispatcher, skillController, slotManager, filter);

        var traitFactory = new TraitFactory(matchConfig.TraitConfig, slotManager.StatusSlots);
        var traitExecutor = new TraitExecutor(traitFactory);
        traitExecutor.ExecuteAllTriat(slotManager.StatusSlots);

        ApplyMastery(); // 마지막에
        ai_main.InitAI_Trait(filter, slotManager, skillController, matchConfig.TeamSize);
    }

    void ApplyMastery()
    {
        new TeamMasteryApplier().ApplyMastery(storage.PickIds.GetTeam(Team.Blue).ToArray(), slotManager.StatusSlots.GetTeam(Team.Blue).ToArray(), gamerRoster.GetTeamMasteries(Team.Blue));
        new TeamMasteryApplier().ApplyMastery(storage.PickIds.GetTeam(Team.Red).ToArray(), slotManager.StatusSlots.GetTeam(Team.Red).ToArray(), gamerRoster.GetTeamMasteries(Team.Red));
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(slotManager.StatusSlots);
        matchUI_Controller.ShowResult(result);
    }
}
