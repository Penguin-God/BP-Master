using System.Linq;

public static class SkillPreviewer
{
    public static SlotStorage<ChampionStatus> SimulatePickCard(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var copiedSlots = CloneSlots(originSlots);
        var copyStatus = champion.Status.DeepCopy();
        var skill = champion.Skill;

        copiedSlots.AddSlot(team, copyStatus);
        if (skill.IsEmpty) return copiedSlots;

        var targets = new SkillTargetService(new HighStatTargetSelector(copiedSlots))
            .GetTargets(team, skill, Factorys.CreateTeamCounter(copiedSlots))
            .Select(x => copiedSlots.GetSlot(x));

        // 비어있는 PhaseActionEventDispatcher를 사용해야 함
        var skillRunner = CreateRunner();
        skillRunner.Run(skill, copyStatus, targets, team);
        return copiedSlots;
    }

    static SkillRunner CreateRunner() => new (new SkillActionFactory(BanPickEventDispatcher.Create(), PhaseEventDispatcher.Create()));

    static SlotStorage<ChampionStatus> CloneSlots(SlotStorage<ChampionStatus> origin)
    {
        var result = new SlotStorage<ChampionStatus>();
        foreach (var slot in origin.GetAllSlotDatas())
            result.AddSlot(slot.Team, origin.GetSlot(slot).DeepCopy());
        return result;
    }
}