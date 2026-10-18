
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
        var beforeScore = ScoreConvertor.Convert(originStats);
        var pickApplyScore = championValueCalculator.Calculate(team, champion, originStats);
        int cardPickDeltaValue = ScoreDeltaCalculator.CalculateScoreDelta(pickApplyScore, team, SpeedValue);

        int bonusDeltaValue = bonusDeltaCalculator.Calculate(beforeScore, beforeScore + pickApplyScore, team);
        return cardPickDeltaValue + bonusDeltaValue;
    }
}
