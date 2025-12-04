using System.Collections.Generic;
using System.Linq;
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

    SlotStorage<Skill> skillSlots;
    public void InitAI_BanPick(PhaseManager phaseManager, GameBanPickStorage storage, SlotStorage<Skill> skillSlots)
    {
        selectorsCreatetor.Init(masteryGenerator.GetTeamMasteryManager(Team));
        var ai = new AI_BanPickAgent(Team, phaseManager, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
        storage.OnPick += OnPick;
        this.skillSlots = skillSlots;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        var teamCount = skillSlots.GetTeamCounter();
        var filter = new SkillTargetFilter(teamCount);
        SelectSkillTarget(filter.FilteringTargetSlots(Team, skillSlots.GetSlot(slotData).Sides).ToList(), teamCount.CalculateTargetCount(Team, EnumCaster.MergeRule(skillSlots.GetSlot(slotData).Rules)));
    }

    IEnumerable<SlotData> SelectSkillTarget(List<SlotData> targetSlots, int targetCount)
    {
        List<SlotData> result = new();
        for (int i = 0; i < targetCount; i++)
        {
            var target = RandomUtil.DrawRandom(targetSlots);
            result.Add(target);
            targetSlots.Remove(target);
        }
        return result;
    }

    public void InitAI_Trait(SkillSlotFilter filter, SlotStorageManager slotManager, SkillUseController skillController, int teamSize)
    {
        var skill_ai = new AI_SkillAgent(Team, filter, slotManager.SkillSlots, skillController, new TargetCounter(teamSize));
        var ai_agent = GetComponent<AI_MonoBehaviourAgent>();
        ai_agent.Init(skill_ai);
        phaseEventDispatcher.OnPhaseSkill += ai_agent.UseTrait;
        if (Team == Team.Blue) skill_ai.UseSkill(Team.Blue);
    }
}