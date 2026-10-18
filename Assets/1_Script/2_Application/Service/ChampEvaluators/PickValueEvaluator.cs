
public class PickValueEvaluator
{
    readonly ChampionValueCalculator championValueCalculator;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    readonly int SpeedValue;

    public PickValueEvaluator(int speedValue, ChampionValueCalculator championValueCalculator, BonusDeltaCalculator bonusDeltaCalculator)
    {
        SpeedValue = speedValue;
        this.championValueCalculator = championValueCalculator;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
    }

    public int Evaluate(Champion champion, Team team, SlotStorage<ChampionStatus> originStats)
    {
        var before = ScoreConvertor.Convert(originStats);
        var pickApplyinfo = championValueCalculator.Calculate(team, champion, originStats);
        int champPickValue = ScoreDeltaCalculator.CalculateScoreDelta(pickApplyinfo, team, SpeedValue);

        int bonusValue = bonusDeltaCalculator.Calculate(before, before + pickApplyinfo, team);
        return champPickValue + bonusValue;
    }
}
