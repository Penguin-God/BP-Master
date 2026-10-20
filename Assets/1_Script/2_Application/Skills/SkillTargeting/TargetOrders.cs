using System;
using System.Collections.Generic;
using System.Linq;

public static class TargetSelector
{
    public static IEnumerable<ChampionStatus> Select(IEnumerable<ChampionStatus> candidates, Func<ChampionStatus, int> sortFun, TargetFindContext context) 
        => candidates.OrderByDescending(sortFun).Take(TargetCountCalculator.CalculateTargetCount(context));
}


public static class TargetOrders
{
    public static Func<ChampionStatus, int> StatOrder(Skill skill) => status => status.GetStatAmount(skill.SkillDatas.First().AmountData.StatType);
    public static Func<ChampionStatus, int> StatOrder(StatType statType) => status => status.GetStatAmount(statType);
}