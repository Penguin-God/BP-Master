public static class ChampionValueCalculator
{
    public static GameScoreInfo Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> before)
    {
        var afterSlots = SkillPreviewer.PreviewSkill(team, champion, before);
        var result = ScoreDeltaCalculator.CalculateStatDelta(before, afterSlots);
        if (Team.Blue == team) result = result with { Blue = result.Blue + CreateStatScore() };
        else if(Team.Red == team) result = result with { Red = result.Red + CreateStatScore() };
        return result;

        ScoreInfo CreateStatScore() => new ScoreInfo(champion.Status.Stat.Attack, champion.Status.Stat.Defense, champion.Status.Stat.Speed);
    }
}