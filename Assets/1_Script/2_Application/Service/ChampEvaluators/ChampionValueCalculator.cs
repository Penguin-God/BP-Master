public static class ChampionValueCalculator
{
    public static GameScoreInfo Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> before)
    {
        var afterSlots = SkillPreviewer.SimulatePickCard(team, champion, before);
        return ScoreDeltaCalculator.CalculateStatDelta(before, afterSlots);
    }
}