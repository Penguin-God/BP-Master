using System;

public class StatChanger : ISkillAction
{
    readonly StatType statType;
    readonly Func<int> _amountProvider;

    public StatChanger(StatType statType, Func<int> amountProvider)
    {
        this.statType = statType;
        _amountProvider = amountProvider;
    }

    public void Do(ChampionStatus target) => target.AddStat(_amountProvider(), statType);
}