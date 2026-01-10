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
        var championRanker = new ChampionRanker(championCatalog, new ChampionValueCalculator(statCalculator, new SkillValueCalculator(new SkillPreviewer(), statusSlots), masteryManager, team));
        var valuePick = new ValuePick(championRanker);
        return new ValuePickLog(valuePick, championRanker);
    }
}
