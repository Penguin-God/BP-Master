
public class ChampionValueCalculator : IChampionEvaluator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly ChampionValueApplier skillValueCalculator;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    readonly Team team;
    readonly SlotStorage<ChampionStatus> originStats;
    public ChampionValueCalculator(ChampionStatValueCalculator statCalculator, ChampionValueApplier skillValueCalculator, BonusDeltaCalculator bonusDeltaCalculator, Team myTeam, SlotStorage<ChampionStatus> originStats)
    {
        this.statCalculator = statCalculator;
        this.skillValueCalculator = skillValueCalculator;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
        this.team = myTeam;
        this.originStats = originStats;
    }

    public int Evaluate(Champion champion)
    {
        var before = ScoreConvertor.Convert(originStats);
        var pickApplyinfo = skillValueCalculator.Calculate(team, champion, originStats);
        int champPickValue = statCalculator.CalcualteTeamStatValue(pickApplyinfo, team);

        int bonusValue = bonusDeltaCalculator.Calculate(before, pickApplyinfo, team);
        return champPickValue + bonusValue;
    }
}
