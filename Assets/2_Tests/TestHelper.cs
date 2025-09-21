using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount) => new Champion(0, "", default, new TraitTargetRule(side, range), new TraitExecutor(new TestAttackChanger(amount), TraitConditionType.None, 0));
    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount, TraitConditionType conditionType, int threshold)
        => new Champion(0, "", default, new TraitTargetRule(side, range), new TraitExecutor(new TestAttackChanger(amount), conditionType, threshold));
    public static Champion CreateChamp(int id, string name) => new Champion(id, name, default, default, null);
    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0) => new Champion(0, "", new ChampionStatData(att, def, speed), default, null);

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(x => CreateBlueSlot(x));
    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(x => CreateRedSlot(x));
}

public class TestAttackChanger : ITraitAction
{
    readonly int Amount;
    public TestAttackChanger(int amount) => Amount = amount;

    public void Do(Champion target) => target.ChangeStat(target.StatData.ChangeAttack(target.StatData.Attack + Amount));
}