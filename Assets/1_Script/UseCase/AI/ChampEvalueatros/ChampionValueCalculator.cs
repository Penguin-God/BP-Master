
public class ChampionValueCalculator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly SkillApplyDeltaCalculator deltaCalculator;
    readonly SkillValueCalculator skillValueCalculator;
    readonly MasteryCollection masteryCollection;
    readonly Team myTeam;
    public ChampionValueCalculator(ChampionStatValueCalculator statCalculator, SkillValueCalculator skillValueCalculator, MasteryCollection masteryCollection, Team myTeam)
    {
        this.statCalculator = statCalculator;
        this.skillValueCalculator = skillValueCalculator;
        this.masteryCollection = masteryCollection;
        this.myTeam = myTeam;
    }

    public ChampionValueCalculator(ChampionStatValueCalculator statCalculator, SkillApplyDeltaCalculator deltaCalculator, MasteryCollection masteryCollection, Team myTeam)
    {
        this.statCalculator = statCalculator;
        this.deltaCalculator = deltaCalculator;
        this.masteryCollection = masteryCollection;
        this.myTeam = myTeam;
    }

    public int Calculate(Champion champion)
    {
        int statScore = statCalculator.CalculateStatValue(champion.Status.Stat);
        int masteryScore = masteryCollection.GetMasteryLevel(champion.Id) * 2;

        var statChangeInfo = deltaCalculator.CalculateApplySkillStat(champion);
        int skillScore = statCalculator.CalcualteTeamStatValue(statChangeInfo, myTeam);

        return statScore + masteryScore + skillScore;
    }
}
