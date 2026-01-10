public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    SlotStorage<ChampionStatus> statusSlots;

    public SkillValueCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
    }

    public GameScoreInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return ScoreDeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }
}