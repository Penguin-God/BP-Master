
public class ChampionValueCalculator : IChampionEvaluator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly ChampionValueApplier skillValueCalculator;
    readonly MasteryCollection masteryCollection;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    readonly Team team;
    readonly SlotStorage<ChampionStatus> originStats;
    public ChampionValueCalculator(ChampionStatValueCalculator statCalculator, ChampionValueApplier skillValueCalculator, MasteryCollection masteryCollection, BonusDeltaCalculator bonusDeltaCalculator, Team myTeam, SlotStorage<ChampionStatus> originStats)
    {
        this.statCalculator = statCalculator;
        this.skillValueCalculator = skillValueCalculator;
        this.masteryCollection = masteryCollection;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
        this.team = myTeam;
        this.originStats = originStats;
    }

    public int Evaluate(Champion champion)
    {
        var before = ScoreConvertor.Convert(originStats);
        int statScore = statCalculator.CalculateStatValue(champion.Status.Stat);
        int masteryScore = masteryCollection.GetMasteryLevel(champion.Id) * 2;

        var statChangeInfo = skillValueCalculator.Calculate(team, champion);
        int skillScore = statCalculator.CalcualteTeamStatValue(statChangeInfo, team);

        // var teamInfo = team == Team.Blue ? statChangeInfo.Blue : statChangeInfo.Red;
        int bonus = bonusDeltaCalculator.Calculate(before, statChangeInfo, team);
        return statScore + masteryScore + skillScore + bonus;
    }
}
