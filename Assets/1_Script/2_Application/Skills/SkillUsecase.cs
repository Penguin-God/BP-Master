using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUsecase
{
    readonly SlotStorage<Champion> pickChampions;
    readonly SkillRunner _skillRunner;
    public event Action<SlotData> OnUseSkill;

    public SkillUsecase(SlotStorage<Champion> pickChampions, SkillRunner skillRunner)
    {
        this.pickChampions = pickChampions;
        _skillRunner = skillRunner;
    }

    public void UseSkill(SlotData skillSlot, IEnumerable<SlotData> targetSlots)
    {
        var champion = pickChampions.GetSlot(skillSlot);
        var targets = targetSlots.Select(x => pickChampions.GetSlot(x).Status);

        _skillRunner.Run(champion.Skill, champion.Status, targets, skillSlot.Team);

        OnUseSkill?.Invoke(skillSlot);
    }

    public void AutoUseSkill(SlotData skillSlot, Func<ChampionStatus, int> sortCriteria)
    {
        var champion = pickChampions.GetSlot(skillSlot);
        if (champion.Skill.IsEmpty)
        {
            OnUseSkill?.Invoke(skillSlot);
            return;
        }

        var rule = EnumCaster.MergeRule(champion.Skill.Rules);
        var teamCount = new TeamCount(pickChampions.GetTeamCount(Team.Blue), pickChampions.GetTeamCount(Team.Red));
        var context = new TargetFindContext(skillSlot.Team, rule, teamCount);

        var candidateStatuses = TargetCountCalculator.FilterTargets(pickChampions, skillSlot.Team, champion.Skill.Sides).Select(c => c.Status);
        var targets = TargetSelector.Select(candidateStatuses, sortCriteria, context);

        _skillRunner.Run(champion.Skill, champion.Status, targets, skillSlot.Team);
        OnUseSkill?.Invoke(skillSlot);
    }
}