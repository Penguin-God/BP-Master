using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(index => CreateBlueSlot(index));

    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(index => CreateRedSlot(index));

    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount)
        => new Champion(
            id: 0,
            name: "",
            statData: default,
            traitDatas: CreateTraitDatas(
                type: TraitType.AttackChanger,
                amount: amount,
                conditionType: TraitConditionType.None,
                threshold: 0,
                side: side,
                range: range
            )
        );

    public static Champion CreateChamp(int id = 0, string name = "", ChampionStatData stat = default, IEnumerable<TraitData> traits = null)
        => new Champion(id, name, stat, traits);

    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0)
        => new Champion(0, "", new ChampionStatData(att, def, speed), null);

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0)
        => new ChampionStatus(new ChampionStatData(att, def, speed));

    public static TraitData CreateTraitData(
        TraitType traitType,
        int amount,
        TraitConditionType traitConditionType = TraitConditionType.None,
        int threshold = 0
    )
        => CreateTraitData(
            type: traitType,
            amount: amount,
            conditionType: traitConditionType,
            threshold: threshold,
            side: default,
            range: default
        );

    public static TraitData CreateAttTraitData(
        int amount,
        TraitConditionType traitConditionType = TraitConditionType.None,
        int threshold = 0,
        Side side = default,
        TargetRange range = default
    )
        => CreateTraitData(type: TraitType.AttackChanger, amount, traitConditionType, threshold, side, range);

    public static TraitData[] CreateTraitDatas(
        TraitType type = default,
        int amount = 0,
        TraitConditionType conditionType = default,
        int threshold = 0,
        Side side = Side.Self,
        TargetRange range = TargetRange.All
    )
        => new[] { CreateTraitData(type, amount, conditionType, threshold, side, range) };

    public static TraitData CreateTraitData(
        TraitType type = default,
        int amount = 0,
        TraitConditionType conditionType = default,
        int threshold = 0,
        Side side = Side.Self,
        TargetRange range = TargetRange.All
    )
        => new TraitData(type, amount, conditionType, threshold, new TraitTargetRule(side, range));
}

public class TestAttackChangeAction : ITraitAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}