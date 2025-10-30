using UnityEngine;

public class AI_Main : MonoBehaviour
{
    Team Team;
    PhaseEventDispatcher phaseEventDispatcher;
    public void Init(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage)
    {
        var ai = new AI_SelectAgent(Team, phaseManager, storage, new RandomBan(), new RandomPick());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
    }

    public void InitAI_Trait(SkillSlotFilter filter, SlotStorageManager slotManager, SkillUseController skillController, AI_MonoBehaviourAgent ai_agent, int teamSize)
    {
        var skill_ai = new AI_TraitAgent(Team, filter, slotManager.SkillSlots, skillController, new TargetCounter(teamSize));
        ai_agent.Init(skill_ai);
        phaseEventDispatcher.OnPhaseSkill += ai_agent.UseTrait;
        if (Team == Team.Blue) skill_ai.UseTrait(Team.Blue);
    }
}