
public class PickValueEvaluator
{
    readonly ChampionStatValueCalculator statCalculator;
    readonly ChampionValueCalculator championValueCalculator;
    readonly BonusDeltaCalculator bonusDeltaCalculator;
    public PickValueEvaluator(ChampionStatValueCalculator statCalculator, ChampionValueCalculator championValueCalculator, BonusDeltaCalculator bonusDeltaCalculator, Team myTeam, SlotStorage<ChampionStatus> originStats)
    {
        this.statCalculator = statCalculator;
        this.championValueCalculator = championValueCalculator;
        this.bonusDeltaCalculator = bonusDeltaCalculator;
    }

    public int Evaluate(Champion champion, Team team, SlotStorage<ChampionStatus> originStats)
    {
        var before = ScoreConvertor.Convert(originStats);
        var pickApplyinfo = championValueCalculator.Calculate(team, champion, originStats);
        int champPickValue = statCalculator.CalcualteTeamStatValue(pickApplyinfo, team);

        int bonusValue = bonusDeltaCalculator.Calculate(before, before + pickApplyinfo, team);
        return champPickValue + bonusValue;
    }
}
