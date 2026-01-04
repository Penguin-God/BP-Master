public class SkillValueCalculator
{
    readonly Team MyTeam;
    readonly SkillPreviewer Previewer;
    readonly ChampionStatValueCalculator StatCalculator;
    readonly SkillApplyDeltaCalculator DeltaCalculator;

    public SkillValueCalculator(Team team, SkillPreviewer previewer, ChampionStatValueCalculator statCalculator, SkillApplyDeltaCalculator deltaCalculator)
    {
        MyTeam = team;
        Previewer = previewer;
        StatCalculator = statCalculator;
        DeltaCalculator = deltaCalculator;
    }

    public int Calculate(Champion champion, SlotStorage<ChampionStatus> originSlots)
    {
        var afterSlots = Previewer.PreviewSkill(champion);
        var changeInfo = DeltaCalculator.CalculateStatDelta(originSlots, afterSlots);
        return StatCalculator.CalcualteTeamStatValue(changeInfo, MyTeam);
    }
}