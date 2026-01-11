using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "BP Master/ValueSelectSO")]
public class ValueSelectSO : AI_SelectorFactory
{
    [SerializeField] int speedValue;
    public override IBanSelector CreateBanSelector() => new RandomBan();

    public override IPickSelector CreatePickSelector()
    {
        var statCalculator = new ChampionStatValueCalculator(speedValue);
        var skillExecutorFactory = new SkillExecutorFactory(new SkillActionFactory(new PhaseActionEventDispatcher()));
        var evaluator = new ChampionValueCalculator(statCalculator, new ChampionValueApplier(new SkillPreviewer(), statusSlots), masteryManager, null, team, null);
        var championRanker = new ChampionRanker(championCatalog, evaluator);
        var valuePick = new ValuePick(championRanker);
        return new ValuePickLog(valuePick, evaluator, championCatalog);
    }
}
