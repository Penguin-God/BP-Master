
public class ChampionValueCalculator : IChampionEvaluator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly SkillValueCalculator skillValueCalculator;
    readonly MasteryCollection masteryCollection;
    readonly Team team;
    public ChampionValueCalculator(ChampionStatValueCalculator statCalculator, SkillValueCalculator skillValueCalculator, MasteryCollection masteryCollection, Team myTeam)
    {
        this.statCalculator = statCalculator;
        this.skillValueCalculator = skillValueCalculator;
        this.masteryCollection = masteryCollection;
        this.team = myTeam;
    }

    public int Evaluate(Champion champion)
    {
        int statScore = statCalculator.CalculateStatValue(champion.Status.Stat);
        int masteryScore = masteryCollection.GetMasteryLevel(champion.Id) * 2;

        var statChangeInfo = skillValueCalculator.Calculate(team, champion);
        int skillScore = statCalculator.CalcualteTeamStatValue(statChangeInfo, team);

        return statScore + masteryScore + skillScore;
    }
}
