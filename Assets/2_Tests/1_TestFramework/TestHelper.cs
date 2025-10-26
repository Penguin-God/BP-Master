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

    public static SlotStorage<IEnumerable<SkillData>> CreateAttTraitSlots(int amount, TraitTargetRule rule)
    {
        SlotStorage<IEnumerable<SkillData>> result = new();
        result.AddSlot(Team.Blue, new SkillData[] { CreateConditionFreeTrait(SkillType.AttackChanger, amount, rule) });
        result.AddSlot(Team.Red, new SkillData[] { CreateConditionFreeTrait(SkillType.AttackChanger, amount, rule) });
        return result;
    }

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(index => CreateBlueSlot(index));

    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(index => CreateRedSlot(index));

    public static ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0, TraitType traitType = TraitType.None) => new ChampionStatus(CreateStat(att, def, speed), traitType);
    public static SkillData[] CreateTraits(params SkillData[] traits) => traits;

    public static SkillData CreateConditionFreeTrait(SkillType type, int amount, TraitTargetRule rule = default) => new SkillData(type, amount, default, rule);

    public static SkillData CreateTraitData(SkillType traitType, int amount, SkillConditionData conditionData, TraitTargetRule traitTargetRule = default)
        => new SkillData(traitType, amount, conditionData, traitTargetRule);

    public static SkillConditionData CreateThresholdCondition(StatConditionType type, int threshold) => new SkillConditionData(type, threshold, ConditionType.Threshold);
    public static SkillConditionData CreateCompareCondition(StatConditionType type) => new SkillConditionData(type, 0, ConditionType.Compare);

    
    public static TraitTargetRule SelfSingleRule => new TraitTargetRule(Side.Self, TargetRange.Single);
    public static TraitTargetRule SelfDouble => new TraitTargetRule(Side.Self, TargetRange.Double);
    public static TraitTargetRule SelfTriple => new TraitTargetRule(Side.Self, TargetRange.Triple);
    public static TraitTargetRule SelfAllRule => new TraitTargetRule(Side.Self, TargetRange.All);

    public static TraitTargetRule OpponentSingleRule => new TraitTargetRule(Side.Opponent, TargetRange.Single);
    public static TraitTargetRule OpponentDoubleRule => new TraitTargetRule(Side.Opponent, TargetRange.Double);
    public static TraitTargetRule OpponentAllRule => new TraitTargetRule(Side.Opponent, TargetRange.All);

    public static TraitTargetRule AllRule => new TraitTargetRule(Side.All, TargetRange.All);

    public static SlotData RedZeroSlot => CreateRedSlot(0);
    public static SlotData RedOneSlot => CreateRedSlot(1);
    public static SlotData BlueZeroSlot => CreateBlueSlot(0);
    public static SlotData BlueOneSlot => CreateBlueSlot(1);
}

public class TestAttackChangeAction : ISkillAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}