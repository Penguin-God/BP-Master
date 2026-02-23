using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "BP Master/ValueSelectSO")]
public class ValueSelectSO : AI_SelectorFactory
{
    [SerializeField] int speedValue;
    [SerializeField] BonusDataFactory bonusDataFactory;
    public override IChampionSelector CreateBanSelector()
    {
        var selector = new ValueSelector(CreateRanker(EnumCaster.GetOppoentTeam(team)));
        return new ValueSelectorLog(selector, CreateValueLogger(EnumCaster.GetOppoentTeam(team)));
    }

    public override IChampionSelector CreatePickSelector()
    {
        var selector = new ValueSelector(CreateRanker(team));
        return new ValueSelectorLog(selector, CreateValueLogger(team));
    }

    PickValueEvaluator CreateEvaluator(Team team)
    {
        var statCalculator = new ChampionStatValueCalculator(speedValue);
        var masteryAppiler = new MasteryApplier(masteryManager);
        return new PickValueEvaluator(statCalculator, new ChampionValueCalculator(new SkillPreviewer(), masteryAppiler), new BonusDeltaCalculator(bonusDataFactory.TeamBonus), team, statusSlots);
    }

    ChampionRanker CreateRanker(Team team) => new ChampionRanker(championCatalog, CreateEvaluator(team));

    ValueLogger CreateValueLogger(Team team) => new ValueLogger(championCatalog, CreateEvaluator(team));
}
