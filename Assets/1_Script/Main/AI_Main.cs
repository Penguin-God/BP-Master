using System.Linq;
using UnityEngine;

public class AI_Main : MonoBehaviour
{
    Team Team;
    PhaseEventDispatcher phaseEventDispatcher;

    [SerializeField] MasteryGenerator masterGenerator;
    [SerializeField] BuildPrioritySO[] buildDatas;

    public void Init(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage)
    {
        MultiPrioritySelector selector = new MultiPrioritySelector(new MasteryManager(masterGenerator.GetTeamMasteries(Team)), buildDatas.Select(x => new PrioritySelector(x.Bans, x.Picks)));
        // var ai = new AI_SelectAgent(Team, phaseManager, storage, new RandomBan(), new StaticValuePick(catalog, evaluator));
        var ai = new AI_SelectAgent(Team, phaseManager, storage, selector, selector);
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