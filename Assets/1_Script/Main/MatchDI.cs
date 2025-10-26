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
    GamerRoster gamerRoster;
    AI_Main ai_main;
    Team playerTeam;
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        this.playerTeam = playerTeam;
        ai_main = new AI_Main(EnumCaster.GetOppoentTeam(playerTeam), phaseEventDispatcher);
        storage = new GameBanPickStorage(championCatalog.AllId);

        matchUI_Controller = GetComponent<MatchUI_Controller>();

        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher);
        utilKey.Init(storage, phaseManager);

        phaseEventDispatcher.OnPhaseSkill += Trait;
        phaseEventDispatcher.OnPhaseDone += OnDone;
        
        gamerRoster  = GetComponent<GamerRoster>();
        gamerRoster.CreateRandomRoster();

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
        var filter = new TraitSlotFilter(slotManager.SkillUseFlagSlot);

        matchUI_Controller.TraitUI_Init(playerTeam, phaseEventDispatcher, skillController, slotManager, filter);

        new Charge(5, slotManager.StatusSlots.GetTeam(Team.Blue)).Do();
        new Charge(5, slotManager.StatusSlots.GetTeam(Team.Red)).Do();

        ApplyMastery(); // 마지막에
        ai_main.InitAI_Trait(filter, slotManager, skillController, GetComponent<AI_MonoBehaviourAgent>());
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


public class AI_Main
{
    public readonly Team Team;
    readonly PhaseEventDispatcher phaseEventDispatcher;
    public AI_Main(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage)
    {
        var ai = new AI_SelectAgent(Team, phaseManager, storage, new RandomSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
    }

    public void InitAI_Trait(TraitSlotFilter filter, SlotStorageManager slotManager, SkillUseController skillController, AI_MonoBehaviourAgent ai_agent)
    {
        var skill_ai = new AI_TraitAgent(Team, filter, slotManager.SkillSlots, skillController, new TargetCounter(5));
        ai_agent.Init(skill_ai);
        phaseEventDispatcher.OnPhaseSkill += ai_agent.UseTrait;
        if (Team == Team.Blue) skill_ai.UseTrait(Team.Blue);
    }
}