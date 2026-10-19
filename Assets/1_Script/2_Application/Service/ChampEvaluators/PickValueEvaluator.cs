public class PickValueEvaluator
{
    readonly MasteryApplier masteryApplier;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    readonly int SpeedValue;

    public PickValueEvaluator(int speedValue, MasteryApplier masteryApplier, BonusDeltaCalculator bonusDeltaCalculator)
    {
        SpeedValue = speedValue;
        this.masteryApplier = masteryApplier;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
    }

    public int Evaluate(Champion champion, Team team, SlotStorage<ChampionStatus> originStats)
    {
        var appliedStatus = champion.Status.DeepCopy();
        masteryApplier.ApplyMastery(champion.Id, appliedStatus);
        var evaluatedChampion = new Champion(champion.Id, champion.Skill, appliedStatus);

        var beforeScore = ScoreConvertor.Convert(originStats);
        var pickApplyScore = ChampionValueCalculator.Calculate(team, evaluatedChampion, originStats);
        int cardPickDeltaValue = ScoreDeltaCalculator.CalculateScoreDelta(pickApplyScore, team, SpeedValue);

        int bonusDeltaValue = bonusDeltaCalculator.Calculate(beforeScore, beforeScore + pickApplyScore, team);
        return cardPickDeltaValue + bonusDeltaValue;
    }
}