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