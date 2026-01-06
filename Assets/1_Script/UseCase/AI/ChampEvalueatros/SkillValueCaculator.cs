public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    readonly SkillApplyDeltaCalculator DeltaCalculator;
    SlotStorage<ChampionStatus> statusSlots;

    public SkillValueCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots, SkillApplyDeltaCalculator deltaCalculator)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
        DeltaCalculator = deltaCalculator;
    }

    public GameStatChangeInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return DeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }
}