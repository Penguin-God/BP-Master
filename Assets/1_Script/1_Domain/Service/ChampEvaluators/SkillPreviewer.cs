using System.Linq;

public class SkillPreviewer
{
    // 비어있는 PhaseActionEventDispatcher를 사용해야 함
    readonly SkillRunner _skillRunner = new SkillRunner(new SkillExecutorFactory(new SkillActionFactory(new PhaseActionEventDispatcher())));
    readonly RandomSkillTargetSelector _skillTargetSelector = new();

    public SlotStorage<ChampionStatus> PreviewSkill(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var copiedSlots = CloneSlots(originSlots);
        if (champion.Skill.IsEmpty) return copiedSlots;

        var targets = _skillTargetSelector
            .SelectSkillTargets(team, champion.Skill, originSlots.GetTeamCounter())
            .Select(x => copiedSlots.GetSlot(x));

        _skillRunner.Run(champion.Skill, champion.Status, targets);
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