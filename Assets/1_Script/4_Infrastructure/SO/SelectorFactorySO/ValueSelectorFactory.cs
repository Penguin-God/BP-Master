using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "BP Master/ValueSelectSO")]
public class ValueSelectSO : AI_SelectorFactory
{
    [SerializeField] int speedValue;
    [SerializeField] BonusDataFactory bonusDataFactory;
    public override IBanSelector CreateBanSelector()
    {
        var valueBan = new ValueBan(CreateRanker(EnumCaster.GetOppoentTeam(team)));
        return new ValueBanLog(valueBan, CreateValueLogger(EnumCaster.GetOppoentTeam(team)));
    }

    public override IPickSelector CreatePickSelector()
    {
        var valuePick = new ValuePick(CreateRanker(team));
        return new ValuePickLog(valuePick, CreateValueLogger(team));
    }

    PickValueEvaluator CreateEvaluator(Team team)
    {
        var statCalculator = new ChampionStatValueCalculator(speedValue);
        var masteryAppiler = new MasteryApplier(masteryManager);
        return new PickValueEvaluator(statCalculator, new ChampionValueApplier(new SkillPreviewer(), masteryAppiler), new BonusDeltaCalculator(bonusDataFactory.TeamBonus), team, statusSlots);
    }

    ChampionRanker CreateRanker(Team team) => new ChampionRanker(championCatalog, CreateEvaluator(team));

    ValueLogger CreateValueLogger(Team team) => new ValueLogger(championCatalog, CreateEvaluator(team));
}
