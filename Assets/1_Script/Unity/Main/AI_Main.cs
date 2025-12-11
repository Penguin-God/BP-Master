using System.Collections;
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
    SkillUseController skillUseController;
    public void InitAI_BanPick(GameBanPickStorage storage, SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        this.skillUseController = skillUseController;
        selectorsCreatetor.Init(masteryGenerator.GetTeamMasteryManager(Team));
        var ai = new AI_BanPickAgent(Team, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
        storage.OnPick += OnPick;
        this.skillSlots = skillSlots;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        StartCoroutine(Co_UseSkill(slotData));
    }

    IEnumerator Co_UseSkill(SlotData slotData)
    {
        yield return new WaitForSeconds(1.5f);
        var teamCount = skillSlots.GetTeamCounter();
        var filter = new SkillTargetFilter(teamCount);
        var useSkill = skillSlots.GetSlot(slotData);
        var targets = SelectSkillTarget(filter.FilteringTargetSlots(Team, useSkill.Sides).ToList(), teamCount.CalculateTargetCount(Team, EnumCaster.MergeRule(useSkill.Rules)));
        skillUseController.UseSkill(slotData, targets, useSkill);
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