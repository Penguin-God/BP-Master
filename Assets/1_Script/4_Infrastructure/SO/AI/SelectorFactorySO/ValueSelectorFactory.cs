using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "BP Master/ValueSelectSO")]
public class ValueSelectSO : AI_SelectorFactory
{
    [SerializeField] int speedValue;
    [SerializeField] BonusDataFactory bonusDataFactory;
    [SerializeField] bool enableLogging;

    public override IChampionSelector CreateBanSelector()
    {
        Team opponentTeam = EnumCaster.GetOppoentTeam(team);
        var selector = new ValueSelector(CreateRanker(opponentTeam));

        if (enableLogging) return new ValueSelectorLog(selector, CreateValueLogger(opponentTeam));
        else return selector;
    }

    public override IChampionSelector CreatePickSelector()
    {
        var selector = new ValueSelector(CreateRanker(team));

        if (enableLogging) return new ValueSelectorLog(selector, CreateValueLogger(team));
        else return selector;
    }

    public PickValueEvaluator CreateEvaluator(Team team)
    {
        var statCalculator = new ChampionStatValueCalculator(speedValue);
        var masteryAppiler = new MasteryApplier(masteryManager);
        return new PickValueEvaluator(statCalculator, new ChampionValueCalculator(new SkillPreviewer(), masteryAppiler), new BonusDeltaCalculator(bonusDataFactory.TeamBonus), team, statusSlots);
    }

    ChampionRanker CreateRanker(Team team) => new ChampionRanker(championCatalog, CreateEvaluator(team));

    ValueLogger CreateValueLogger(Team team) => new ValueLogger(championCatalog, CreateEvaluator(team));
}