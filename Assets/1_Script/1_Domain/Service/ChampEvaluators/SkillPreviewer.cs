using System.Linq;

public class SkillPreviewer
{
    private readonly SkillRunner _skillRunner = new SkillRunner(new SkillExecutorFactory(new SkillActionFactory(new PhaseActionEventDispatcher())));
    private readonly RandomSkillTargetSelector _skillTargetSelector = new();

    public SkillPreviewer(SkillRunner skillRunner)
    {
        _skillRunner = skillRunner;
    }

    public SkillPreviewer(){}

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

    private SlotStorage<ChampionStatus> CloneSlots(SlotStorage<ChampionStatus> origin)
    {
        var result = new SlotStorage<ChampionStatus>();
        foreach (var slot in origin.GetAllSlotDatas())
            result.AddSlot(slot.Team, origin.GetSlot(slot).DeepCopy());
        return result;
    }
}