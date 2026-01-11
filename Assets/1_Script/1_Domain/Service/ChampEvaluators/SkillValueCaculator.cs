public class ChampionValueApplier
{
    readonly SkillPreviewer Previewer;
    SlotStorage<ChampionStatus> statusSlots;

    public ChampionValueApplier(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
    }

    readonly MasteryApplier masteryApplier;
    public ChampionValueApplier(SkillPreviewer previewer, MasteryApplier masteryApplier)
    {
        Previewer = previewer;
        this.masteryApplier = masteryApplier;
    }

    public GameScoreInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return ScoreDeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }


    public GameScoreInfo Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> before)
    {
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
        var afterSlots = Previewer.PreviewSkill(team, champion, before);
        var result = ScoreDeltaCalculator.CalculateStatDelta(before, afterSlots);
        var blue = GetTeamAddStatScore(Team.Blue);
        var red = GetTeamAddStatScore(Team.Red);
        result += new GameScoreInfo(blue, red);
        return result;

        ScoreInfo GetTeamAddStatScore(Team statTeam) => team == statTeam ? new ScoreInfo(champion.Status.Stat.Attack, champion.Status.Stat.Defense, champion.Status.Stat.Speed) : new ScoreInfo(0, 0, 0);
    }
}