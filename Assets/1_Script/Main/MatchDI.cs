using UnityEngine;

public class MatchDI : MonoBehaviour
{
    SlotStorageManager slotManager;
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    GameBanPickStorage storage;

    PhaseManager phaseManager;
    PhaseEventDispatcher phaseEventDispatcher = new PhaseEventDispatcher();
    MatchUI_Controller matchUI_Controller;
    IdStorageConverter storageFactory;
    GamerRoster gamerRoster;

    Team playerTeam;
    Team aiTeam;
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        this.playerTeam = playerTeam;
        aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        storage = new GameBanPickStorage(championCatalog.AllId);

        matchUI_Controller = GetComponent<MatchUI_Controller>();

        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher);
        utilKey.Init(storage, phaseManager);

        phaseEventDispatcher.OnPhaseSkill += Trait;
        phaseEventDispatcher.OnPhaseDone += OnDone;
        storageFactory = new IdStorageConverter(championCatalog);

        gamerRoster  = GetComponent<GamerRoster>();
        gamerRoster.SetRandomRoster(championCatalog);

        matchUI_Controller.Init(storage, phaseManager, storageFactory, phaseEventDispatcher); // start보다 먼저

        var ai = new AI_SelectAgent(aiTeam, phaseManager, storage, new RandomSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;

        phaseManager.Start();
    }

    bool initTrait;
    void Trait(Team team)
    {
        if (initTrait) return;
        initTrait = true;
        slotManager = new SlotStorageManager(storage, storageFactory);

        var traitFacade = new SkillUseOrchestrator(slotManager.StatusSlots);
        traitFacade.OnUseSkill += slot => slotManager.SkillUseFlagSlot.ChangeSlot(slot, true);
        traitFacade.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        var filter = new TraitSlotFilter(slotManager.SkillUseFlagSlot);

        var trait_ai = new AI_TraitAgent(aiTeam, filter, slotManager.SkillSlots, traitFacade, new TargetCounter(5));
        AI_MonoBehaviourAgent aI_Mono = GetComponent<AI_MonoBehaviourAgent>();
        aI_Mono.Init(trait_ai);
        phaseEventDispatcher.OnPhaseSkill += GetComponent<AI_MonoBehaviourAgent>().UseTrait;

        matchUI_Controller.TraitUI_Init(playerTeam, phaseEventDispatcher, traitFacade, slotManager, filter);

        ApplyMastery(); // 마지막에
        if(aiTeam == Team.Blue) trait_ai.UseTrait(Team.Blue);
    }

    void ApplyMastery() => new TeamMasteryApplier().Apply(gamerRoster.Rosters, storage.PickIds, slotManager.StatusSlots);

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(slotManager.StatusSlots);
        matchUI_Controller.ShowResult(result);
    }
}
