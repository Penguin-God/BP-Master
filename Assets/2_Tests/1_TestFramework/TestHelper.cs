using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount)
        => new Champion(0, "", default,
            CreateTraitDatas(type: TraitType.AttackChanger, amount: amount, side: side, range: range));

    public static Champion CreateChamp(int id = 0, string name = "", ChampionStatData stat = default, IEnumerable<TraitData> traits = null) => new Champion(id, name, stat, traits);
    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0) => new Champion(0, "", new ChampionStatData(att, def, speed), new TraitData[0]);

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(x => CreateBlueSlot(x));
    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(x => CreateRedSlot(x));

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0) => new ChampionStatus(new ChampionStatData(att, def, speed));
    public static TraitData CreateTraitData(TraitType traitType, int amount, TraitConditionType traitConditionType = TraitConditionType.None, int threshold = 0) => new TraitData(traitType, amount, traitConditionType, threshold, default);

    public static TraitData CreateAttTraitData(int amount, TraitConditionType traitConditionType = 0, int threshold = 0, Side side = 0, TargetRange range = 0)
        => new TraitData(TraitType.AttackChanger, amount, traitConditionType, threshold, new TraitTargetRule(side, range));

    public static TraitData[] CreateTraitDatas(TraitType type = 0, int amount = 0, TraitConditionType conditionType = 0, int threshold = 0, Side side = Side.Self, TargetRange range = TargetRange.All)
        => new TraitData[]{ CreateTraitData(type, amount, conditionType, threshold, side, range) };

    public static TraitData CreateTraitData(TraitType type = 0, int amount = 0, TraitConditionType conditionType = 0, int threshold = 0, Side side = Side.Self, TargetRange range = TargetRange.All)
        => new TraitData(type, amount, conditionType, threshold, new TraitTargetRule(side, range));
}

public class TestAttackChangeAction : ITraitAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}