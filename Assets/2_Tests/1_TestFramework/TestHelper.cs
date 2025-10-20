using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static SlotStorage<ChampionStatus> CreateOneSlotStatus(int att = 0, int def = 0, int speed = 0)
    {
        SlotStorage<ChampionStatus> result = new();
        result.AddSlot(Team.Blue, CreateStatus(att, def, speed));
        result.AddSlot(Team.Red, CreateStatus(att, def, speed));
        return result;
    }

    public static SlotStorage<ChampionStatus> CreateTwoSlotStatus(int att = 0, int def = 0, int speed = 0)
    {
        SlotStorage<ChampionStatus> result = CreateOneSlotStatus(att, def, speed);
        result.AddSlot(Team.Blue, CreateStatus(att, def, speed));
        result.AddSlot(Team.Red, CreateStatus(att, def, speed));
        return result;
    }

    public static SlotStorage<TraitApplier> CreateOneSlotApplier(SlotStorage<ChampionStatus> statuses)
    {
        SlotStorage<TraitApplier> result = new();
        result.AddSlot(Team.Blue, new TraitApplier(statuses, BlueZeroSlot));
        result.AddSlot(Team.Red, new TraitApplier(statuses, RedZeroSlot));
        return result;
    }

    public static SlotStorage<TraitApplier> CreateTwoSlotApplier(SlotStorage<ChampionStatus> statuses)
    {
        SlotStorage<TraitApplier> result = CreateOneSlotApplier(statuses);
        result.AddSlot(Team.Blue, new TraitApplier(statuses, BlueZeroSlot));
        result.AddSlot(Team.Red, new TraitApplier(statuses, RedZeroSlot));
        return result;
    }

    public static SlotStorage<IEnumerable<TraitData>> CreateAttTraitSlots(int amount, TraitTargetRule rule)
    {
        SlotStorage<IEnumerable<TraitData>> result = new();
        result.AddSlot(Team.Blue, new TraitData[] { CreateConditionFreeTrait(TraitType.AttackChanger, amount, rule) });
        result.AddSlot(Team.Red, new TraitData[] { CreateConditionFreeTrait(TraitType.AttackChanger, amount, rule) });
        return result;
    }

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

    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0) => new Champion(0, "", CreateStat(att, def, speed), null);
    public static ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0) => new ChampionStatus(CreateStat(att, def, speed));
    public static TraitData[] CreateTraits(params TraitData[] traits) => traits;

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
        TargetRange range = TargetRange.All,
        ConditionCheckerType checkerType = ConditionCheckerType.None
    )
        => new TraitData(type, amount, new TraitConditionData(conditionType, threshold, checkerType), new TraitTargetRule(side, range));

    public static TraitData CreateConditionFreeTrait(TraitType type, int amount, TraitTargetRule rule = default) => new TraitData(type, amount, default, rule);

    public static TraitData CreateTraitData(TraitType traitType, int amount, TraitConditionData conditionData, TraitTargetRule traitTargetRule = default)
        => new TraitData(traitType, amount, conditionData, traitTargetRule);

    public static TraitConditionData CreateThresholdCondition(TraitConditionType type, int threshold) => new TraitConditionData(type, threshold, ConditionCheckerType.Threshold);
    public static TraitConditionData CreateCompareCondition(TraitConditionType type) => new TraitConditionData(type, 0, ConditionCheckerType.Compare);

    public static TraitTargetRule SelfAllRule => new TraitTargetRule(Side.Self, TargetRange.All);
    public static TraitTargetRule OpponentAllRule => new TraitTargetRule(Side.Opponent, TargetRange.All);
    
    public static TraitTargetRule SelfSingleRule => new TraitTargetRule(Side.Self, TargetRange.Single);
    public static TraitTargetRule OpponentSingleRule => new TraitTargetRule(Side.Opponent, TargetRange.Single);
    public static TraitTargetRule AllRule => new TraitTargetRule(Side.All, TargetRange.All);

    public static SlotData RedZeroSlot => CreateRedSlot(0);
    public static SlotData RedOneSlot => CreateRedSlot(1);
    public static SlotData BlueZeroSlot => CreateBlueSlot(0);
    public static SlotData BlueOneSlot => CreateBlueSlot(1);
}

public class TestAttackChangeAction : ITraitAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}