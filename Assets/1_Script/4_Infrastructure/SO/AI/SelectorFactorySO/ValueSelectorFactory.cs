using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "AI/Selector//Value")]
public class ValueSelectSO : AI_SelectorSO
{
    [SerializeField] int speedValue;
    [SerializeField] TeamBonusDataSO bonusDataFactory;
    [SerializeField] bool enableLogging;

    public override IChampionSelector CreateBanSelector()
    {
        Team opponentTeam = EnumCaster.GetOppoentTeam(team);
        var selector = CreateValueSelector(opponentTeam);

        if (enableLogging) return new ValueSelectorLog(selector);
        else return selector;
    }

    public override IChampionSelector CreatePickSelector()
    {
        var selector = CreateValueSelector(team);

        if (enableLogging) return new ValueSelectorLog(selector);
        else return selector;
    }

    public PickValueEvaluator CreateEvaluator(Team team)
    {
        var masteryAppiler = new MasteryApplier(masteryManager);
        return new PickValueEvaluator(speedValue, new ChampionValueCalculator(masteryAppiler), new BonusDeltaCalculator(bonusDataFactory.TeamBonus));
    }

    ValueSelector CreateValueSelector(Team team) => new (championCatalog, (card) => CreateEvaluator(team).Evaluate(card, team, statusSlots));
}