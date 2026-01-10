public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    readonly SkillApplyDeltaCalculator DeltaCalculator = new SkillApplyDeltaCalculator();
    SlotStorage<ChampionStatus> statusSlots;

    public SkillValueCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
    }

    public GameStatChangeInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return DeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }
}