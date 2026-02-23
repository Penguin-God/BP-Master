
public class PickValueEvaluator : IChampionEvaluator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly ChampionValueCalculator championValueCalculator;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    readonly Team team;
    readonly SlotStorage<ChampionStatus> originStats;
    public PickValueEvaluator(ChampionStatValueCalculator statCalculator, ChampionValueCalculator championValueCalculator, BonusDeltaCalculator bonusDeltaCalculator, Team myTeam, SlotStorage<ChampionStatus> originStats)
    {
        this.statCalculator = statCalculator;
        this.championValueCalculator = championValueCalculator;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
        this.team = myTeam;
        this.originStats = originStats;
    }

    public int Evaluate(Champion champion)
    {
        var before = ScoreConvertor.Convert(originStats);
        var pickApplyinfo = championValueCalculator.Calculate(team, champion, originStats);
        int champPickValue = statCalculator.CalcualteTeamStatValue(pickApplyinfo, team);

        int bonusValue = bonusDeltaCalculator.Calculate(before, before + pickApplyinfo, team);
        return champPickValue + bonusValue;
    }
}
