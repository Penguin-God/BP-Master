using System;
using System.Collections.Generic;
using System.Linq;


public class RandomSkillTargetSelector : ISkillTargetSelector
{
    public IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill) => candidates.OrderBy(x => Guid.NewGuid()).Take(count);
}

public class HighStatTargetSelector : ISkillTargetSelector
{
    readonly SlotStorage<ChampionStatus> statusStorage;

    public HighStatTargetSelector(SlotStorage<ChampionStatus> statusStorage)
    {
        this.statusStorage = statusStorage;
    }

    public IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill)
    {
        if (skill.IsEmpty) return new SlotData[0];

        return candidates
            .OrderByDescending(slot => statusStorage.GetSlot(slot).Stat.GetStatValue(skill.SkillDatas.First().AmountData.StatType))
            .Take(count);
    }
}



public static class TargetSelector
{
    public static IEnumerable<ChampionStatus> Select(IEnumerable<ChampionStatus> candidates, Func<ChampionStatus, int> sortFun, int count) => candidates.OrderByDescending(sortFun).Take(count);

    public static IEnumerable<ChampionStatus> Select(IEnumerable<ChampionStatus> candidates, Func<ChampionStatus, int> sortFun, TargetFindContext context) 
        => candidates.OrderByDescending(sortFun).Take(TargetCountCalculator.CalculateTargetCount(context));
}


public static class TargetOrders
{
    public static Func<ChampionStatus, int> StatOrder(Skill skill) => status => status.GetStatAmount(skill.SkillDatas.First().AmountData.StatType);
    public static Func<ChampionStatus, int> StatOrder(StatType statType) => status => status.GetStatAmount(statType);
    public static Func<ChampionStatus, int> RandomOrder() => _ => Guid.NewGuid().GetHashCode();
}