using System;
using System.Collections.Generic;
using System.Linq;


public class RandomSkillTargetSelector : ISkillTargetSelector
{
    public IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill)
    {
        return candidates.OrderBy(x => Guid.NewGuid()).Take(count);
    }
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
        var percentData = skill.SkillDatas.First();

        return candidates
            .OrderByDescending(slot => statusStorage.GetSlot(slot).Stat.GetStatValue(percentData.AmountData.StatType))
            .Take(count);
    }
}