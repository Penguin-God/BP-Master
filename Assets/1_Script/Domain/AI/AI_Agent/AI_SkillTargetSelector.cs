using System.Collections.Generic;

public class AI_SkillTargetSelector
{
    public IEnumerable<SlotData> SelectSkillTargets(List<SlotData> targetSlots, int targetCount)
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
}

interface ISkillTargetSelector
{

}