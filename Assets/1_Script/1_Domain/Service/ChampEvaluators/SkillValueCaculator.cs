public class ChampionValueCalculator
{
    readonly MasteryApplier masteryApplier;
    public ChampionValueCalculator(MasteryApplier masteryApplier)
    {
        this.masteryApplier = masteryApplier;
    }

    public GameScoreInfo Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> before)
    {
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
        var afterSlots = SkillPreviewer.PreviewSkill(team, champion, before);
        var result = ScoreDeltaCalculator.CalculateStatDelta(before, afterSlots);
        var blue = GetTeamAddStatScore(Team.Blue);
        var red = GetTeamAddStatScore(Team.Red);
        result += new GameScoreInfo(blue, red);
        return result;

        ScoreInfo GetTeamAddStatScore(Team statTeam) => team == statTeam ? new ScoreInfo(champion.Status.Stat.Attack, champion.Status.Stat.Defense, champion.Status.Stat.Speed) : new ScoreInfo(0, 0, 0);
    }
}