public class SkillValueCalculator
{
    readonly SkillPreviewer Previewer;
    SlotStorage<ChampionStatus> statusSlots;

    public SkillValueCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> statusSlots)
    {
        Previewer = previewer;
        this.statusSlots = statusSlots;
    }

    readonly MasteryCollection masteryCollection;
    public SkillValueCalculator(SkillPreviewer previewer, MasteryCollection masteryCollection)
    {
        Previewer = previewer;
        this.masteryCollection = masteryCollection;
    }

    public GameScoreInfo Calculate(Team team, Champion champion)
    {
        var afterSlots = Previewer.PreviewSkill(team, champion, statusSlots);
        return ScoreDeltaCalculator.CalculateStatDelta(statusSlots, afterSlots);
    }


    public GameScoreInfo Calculate(Team team, Champion champion, SlotStorage<ChampionStatus> before)
    {
        new MasteryApplier().ApplyStatChange(champion.Status, masteryCollection.GetMasteryLevel(champion.Id));
        var afterSlots = Previewer.PreviewSkill(team, champion, before);
        var result = ScoreDeltaCalculator.CalculateStatDelta(before, afterSlots);
        var blue = GetTeamAddStatScore(Team.Blue);
        var red = GetTeamAddStatScore(Team.Red);
        result += new GameScoreInfo(blue, red);
        return result;

        ScoreInfo GetTeamAddStatScore(Team statTeam) => team == statTeam ? new ScoreInfo(champion.Status.Stat.Attack, champion.Status.Stat.Defense, champion.Status.Stat.Speed) : new ScoreInfo(0, 0, 0);
    }
}