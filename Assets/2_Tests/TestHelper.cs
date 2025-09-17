using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static ChampionSlot CreateBlueSlot(int index) => new ChampionSlot(Team.Blue, index);
    public static ChampionSlot CreateRedSlot(int index) => new ChampionSlot(Team.Red, index);

    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount) => new Champion(0, "", default, new TraitTargetRule(side, range), new TraitExecutor(new TestAttackChanger(amount), TraitConditionType.None, 0));
    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount, TraitConditionType conditionType, int threshold)
        => new Champion(0, "", default, new TraitTargetRule(side, range), new TraitExecutor(new TestAttackChanger(amount), conditionType, threshold));
    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0) => new Champion(0, "", new ChampionStatData(att, def, speed), default, null);

    public static IEnumerable<ChampionSlot> CreateBlueSlots(params int[] indexs) => indexs.Select(x => CreateBlueSlot(x));
    public static IEnumerable<ChampionSlot> CreateRedSlots(params int[] indexs) => indexs.Select(x => CreateRedSlot(x));
}

public class TestAttackChanger : ITraitAction
{
    readonly int Amount;
    public TestAttackChanger(int amount) => Amount = amount;

    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeAttack(target.StatData.Attack + Amount));
}