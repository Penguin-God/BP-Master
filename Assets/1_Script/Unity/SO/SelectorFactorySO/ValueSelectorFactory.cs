using UnityEngine;

[CreateAssetMenu(fileName = "ValueSelectSO", menuName = "BP Master/ValueSelectSO")]
public class ValueSelectSO : AI_SelectorFactory
{
    [SerializeField] int speedValue;
    public override IBanSelector CreateBanSelector() => new RandomBan();

    public override IPickSelector CreatePickSelector()
    {
        var statCalculator = new ChampionStatValueCalculator(speedValue);
        var skillExecutorFactory = new SkillExecutorFactory(new SkillActionFactory(new PhaseActionEventDispatcher())); // 실제 사용중인 객체로 하면 안됨
        var skillValueCalculator = new SkillApplyDeltaCalculator(new SkillPreviewer(team, skillExecutorFactory, statusSlots), statusSlots);
        return new ValuePickLog(championCatalog, statCalculator, skillValueCalculator, masteryManager, team);
    }
}
