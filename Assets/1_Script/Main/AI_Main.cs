using System.Linq;
using UnityEngine;

public class AI_Main : MonoBehaviour
{
    Team Team;
    PhaseEventDispatcher phaseEventDispatcher;

    [SerializeField] ChampionRepository championRepository;
    [SerializeField] MasteryGenerator gamerRoster;
    [SerializeField] BuildPrioritySO deckData;
    ChampionCatalog catalog;
    StaticValueEvaluator evaluator;
    void Start()
    {
        catalog = new ChampionCatalog(championRepository.AllChampion.ToDictionary(x => x.Id, x => x.CreateData()));    
    }

    public void Init(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
        evaluator = new StaticValueEvaluator(gamerRoster.GetTeamMasteries(team));
    }

    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage)
    {
        PrioritySelector prioritySelector = new PrioritySelector(deckData.Bans, deckData.Picks);
        // var ai = new AI_SelectAgent(Team, phaseManager, storage, new RandomBan(), new StaticValuePick(catalog, evaluator));
        var ai = new AI_SelectAgent(Team, phaseManager, storage, prioritySelector, prioritySelector);
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
    }

    public void InitAI_Trait(SkillSlotFilter filter, SlotStorageManager slotManager, SkillUseController skillController, int teamSize)
    {
        var skill_ai = new AI_TraitAgent(Team, filter, slotManager.SkillSlots, skillController, new TargetCounter(teamSize));
        var ai_agent = GetComponent<AI_MonoBehaviourAgent>();
        ai_agent.Init(skill_ai);
        phaseEventDispatcher.OnPhaseSkill += ai_agent.UseTrait;
        if (Team == Team.Blue) skill_ai.UseTrait(Team.Blue);
    }
}