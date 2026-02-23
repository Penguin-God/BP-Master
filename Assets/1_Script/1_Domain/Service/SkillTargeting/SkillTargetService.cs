using System.Collections.Generic;

public interface ISkillTargetSelector
{
    IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill);
}

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
        var filter = new SkillTargetFilter(calculator);
        var candidates = filter.FilteringTargetSlots(casterTeam, skill.Sides);

        return selector.SelectTargets(candidates, count, skill);
    }
}