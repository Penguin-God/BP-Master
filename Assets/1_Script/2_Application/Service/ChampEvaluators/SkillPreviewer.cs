using System.Linq;

public static class SkillPreviewer
{
    public static SlotStorage<ChampionStatus> PreviewSkill(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var copiedSlots = CloneSlots(originSlots);
        if (champion.Skill.IsEmpty) return copiedSlots;

        var targets = new SkillTargetService(new HighStatTargetSelector(originSlots))
            .GetTargets(team, champion.Skill, Factorys.CreateTeamCounter(originSlots))
            .Select(x => copiedSlots.GetSlot(x));

        // 비어있는 PhaseActionEventDispatcher를 사용해야 함
        var skillRunner = CreateRunner(BanPickEventDispatcher.Create(), PhaseEventDispatcher.Create());
        skillRunner.Run(champion.Skill, champion.Status, targets, team);
        return copiedSlots;
    }

    public static SkillRunner CreateRunner(BanPickEventDispatcher phaseActionEventDispatcher, PhaseEventDispatcher phaseEventDispatcher)
    {
        return new SkillRunner(new SkillActionFactory(phaseActionEventDispatcher, phaseEventDispatcher), new SkillCondtionFactory());
    }

    static SlotStorage<ChampionStatus> CloneSlots(SlotStorage<ChampionStatus> origin)
    {
        var result = new SlotStorage<ChampionStatus>();
        foreach (var slot in origin.GetAllSlotDatas())
            result.AddSlot(slot.Team, origin.GetSlot(slot).DeepCopy());
        return result;
    }
}