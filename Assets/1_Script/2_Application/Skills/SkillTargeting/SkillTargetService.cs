using System.Collections.Generic;

public class SkillTargetService
{
    readonly ISkillTargetSelector selector;

    public SkillTargetService(ISkillTargetSelector selector)
    {
        this.selector = selector;
    }

    public IEnumerable<SlotData> GetTargets(Team casterTeam, Skill skill, TargetCountCalculator calculator)
    {
        int count = calculator.CalculateTargetCount(casterTeam, EnumCaster.MergeRule(skill.Rules));
        var candidates = calculator.FilteringTargetSlots(casterTeam, skill.Sides);

        return selector.SelectTargets(candidates, count, skill);
    }
}