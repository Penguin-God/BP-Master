using System.Collections.Generic;
using System.Linq;

public class SkillPreviewer
{
    readonly SkillExecutorFactory skillExecutorFactory;
    readonly SlotStorage<ChampionStatus> originSlots;
    public SkillPreviewer(SkillExecutorFactory skillExecutorFactory, SlotStorage<ChampionStatus> originSlots)
    {
        this.skillExecutorFactory = skillExecutorFactory;
        this.originSlots = originSlots;
    }

    public SlotStorage<ChampionStatus> PreviewSkill(Champion champion, IEnumerable<SlotData> targetSlots)
    {
        var copiedSlots = CloneSlots(originSlots);

        var targets = targetSlots.Select(x => copiedSlots.GetSlot(x));
        foreach (var skillData in champion.Skill.SkillDatas)
        {
            var executor = skillExecutorFactory.CreateExecutor(skillData, champion.Status);
            executor.ExecuteSkill(targets);
        }
        return copiedSlots;
    }

    SlotStorage<ChampionStatus> CloneSlots(SlotStorage<ChampionStatus> origin)
    {
        var result = new SlotStorage<ChampionStatus>();
        foreach (var slot in origin.GetAllSlotDatas())
            result.AddSlot(slot.Team, origin.GetSlot(slot).DeepCopy());
        return result;
    }
}
