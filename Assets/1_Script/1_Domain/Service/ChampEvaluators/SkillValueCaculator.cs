public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    readonly ScoreDeltaCalculator DeltaCalculator = new ScoreDeltaCalculator();
    SlotStorage<ChampionStatus> statusSlots;

    public SkillValueCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
    }

    public GameScoreInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return DeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }
}