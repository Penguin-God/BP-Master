using UnityEngine;

public class AI_Main : MonoBehaviour
{
    Team Team;
    PhaseEventDispatcher phaseEventDispatcher;

    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] SelectorsCreatetorSO selectorsCreatetor;

    public void Init(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage)
    {
        selectorsCreatetor.Init(masteryGenerator.GetTeamMasteryManager(Team));
        var ai = new AI_BanPickAgent(Team, phaseManager, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
    }

    public void InitAI_Trait(SkillSlotFilter filter, SlotStorageManager slotManager, SkillUseController skillController, int teamSize)
    {
        var skill_ai = new AI_SkillAgent(Team, filter, slotManager.SkillSlots, skillController, new TargetCounter(teamSize));
        var ai_agent = GetComponent<AI_MonoBehaviourAgent>();
        ai_agent.Init(skill_ai);
        phaseEventDispatcher.OnPhaseSkill += ai_agent.UseTrait;
        if (Team == Team.Blue) skill_ai.UseTrait(Team.Blue);
    }
}