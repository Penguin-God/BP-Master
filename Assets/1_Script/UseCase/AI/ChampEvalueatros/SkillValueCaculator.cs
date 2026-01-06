public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    readonly ChampionStatValueCalculator StatCalculator;
    readonly SkillApplyDeltaCalculator DeltaCalculator;

    public SkillValueCalculator(SkillPreviewer previewer, ChampionStatValueCalculator statCalculator, SkillApplyDeltaCalculator deltaCalculator)
    {
        Previewer = previewer;
        StatCalculator = statCalculator;
        DeltaCalculator = deltaCalculator;
    }

    public int Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion);
        var changeInfo = DeltaCalculator.CalculateStatDelta(originSlots, afterSlots);
        return StatCalculator.CalcualteTeamStatValue(changeInfo, team);
    }
}