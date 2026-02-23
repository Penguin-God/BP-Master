using System.Linq;

public class SkillPreviewer
{
    readonly SkillTargetService skillTargetService = new SkillTargetService(new RandomSkillTargetSelector());
    public SlotStorage<ChampionStatus> PreviewSkill(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var copiedSlots = CloneSlots(originSlots);
        if (champion.Skill.IsEmpty) return copiedSlots;

        var targets = skillTargetService
            .GetTargets(team, champion.Skill, originSlots.GetTeamCounter())
            .Select(x => copiedSlots.GetSlot(x));

        // 비어있는 PhaseActionEventDispatcher를 사용해야 함
        var skillRunner = SkillRunnerFactory.CreateRunner(new BanPickEventDispatcher(), new PhaseEventDispatcher());
        skillRunner.Run(champion.Skill, champion.Status, targets, team);
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