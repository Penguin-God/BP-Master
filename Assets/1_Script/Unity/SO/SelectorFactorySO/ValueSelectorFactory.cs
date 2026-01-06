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
        var valuePick = new ValuePick(championCatalog, new ChampionValueCalculator(statCalculator, new SkillValueCalculator(new SkillPreviewer(), statusSlots), masteryManager, team));
        return new ValuePickLog(valuePick);
    }
}
