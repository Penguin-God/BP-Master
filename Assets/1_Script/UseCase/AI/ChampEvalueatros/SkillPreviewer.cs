using System.Linq;

public class SkillPreviewer
{
    readonly SkillExecutorFactory skillExecutorFactory;
    readonly SlotStorage<ChampionStatus> originSlots;
    readonly AI_SkillTargetSelector skillTargetSelector = new AI_SkillTargetSelector();
    readonly Team Team;

    public SkillPreviewer(Team team, SkillExecutorFactory skillExecutorFactory, SlotStorage<ChampionStatus> originSlots)
    {
        Team = team;
        this.skillExecutorFactory = skillExecutorFactory;
        this.originSlots = originSlots;
    }

    public SkillPreviewer()
    {

    }

    public SlotStorage<ChampionStatus> PreviewSkill(Champion champion)
    {
        var copiedSlots = CloneSlots(originSlots);
        var targets = skillTargetSelector.SelectSkillTargets(Team, champion.Skill, originSlots.GetTeamCounter()).Select(x => copiedSlots.GetSlot(x));
        foreach (var skillData in champion.Skill.SkillDatas)
        {
            var executor = skillExecutorFactory.CreateExecutor(skillData, champion.Status);
            executor.ExecuteSkill(targets);
        }
        return copiedSlots;
    }

    public SlotStorage<ChampionStatus> PreviewSkill(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var copiedSlots = CloneSlots(originSlots);
        var targets = skillTargetSelector.SelectSkillTargets(team, champion.Skill, originSlots.GetTeamCounter()).Select(x => copiedSlots.GetSlot(x));
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
