using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static SlotStorage<ChampionStatus> CreateOneSlotStatus(int att = 0, int def = 0, int speed = 0, TraitType traitType = TraitType.None)
    {
        SlotStorage<ChampionStatus> result = new();
        result.AddSlot(Team.Blue, CreateStatus(att, def, speed, traitType));
        result.AddSlot(Team.Red, CreateStatus(att, def, speed, traitType));
        return result;
    }

    public static SlotStorage<ChampionStatus> CreateTwoSlotStatus(int att = 0, int def = 0, int speed = 0)
    {
        SlotStorage<ChampionStatus> result = CreateOneSlotStatus(att, def, speed);
        result.AddSlot(Team.Blue, CreateStatus(att, def, speed));
        result.AddSlot(Team.Red, CreateStatus(att, def, speed));
        return result;
    }



    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(index => CreateBlueSlot(index));

    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(index => CreateRedSlot(index));

    public static ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0, TraitType traitType = TraitType.None) => new ChampionStatus(CreateStat(att, def, speed), traitType);
    public static SkillData[] CreateSkills(params SkillData[] traits) => traits;

    public static SkillData CreateConditionFreeSkill(SkillType type, int amount, SkillTargetRule rule = default) => CreateValueSkillData(type, amount, default, rule);

    public static SkillData CreateValueSkillData(SkillType traitType, int amount, SkillConditionData conditionData = default, SkillTargetRule traitTargetRule = default)
        => new SkillData(traitType, new SkillAmountData(AmountType.Value, amount, 0, 0), conditionData, traitTargetRule);

    public static Skill CreateValueSkill(SkillType skillType, int amount, SkillConditionData conditionData = default, SkillTargetRule rule = default)
        => new Skill(CreateSkills(CreateValueSkillData(skillType, amount, conditionData, rule)));

    public static SkillConditionData CreateThresholdCondition(StatConditionType type, int threshold) => CreateConditionData(ConditionType.Threshold, statType: type, threshold: threshold);
    public static SkillConditionData CreateCompareCondition(StatConditionType type) => CreateConditionData(ConditionType.Compare, statType: type);
    public static SkillConditionData CreateConditionData(ConditionType conditionType, StatConditionType statType = StatConditionType.None, int threshold = 0, TraitType traitType = TraitType.None)
        => new SkillConditionData(statType, threshold, traitType, conditionType);

    static SkillTargetRule CreateRule(Side side, TargetRange range) => new SkillTargetRule(side, range);
    public static SkillTargetRule SelfSingleRule => CreateRule(Side.Self, TargetRange.Single);
    public static SkillTargetRule SelfDouble => CreateRule(Side.Self, TargetRange.Double);
    public static SkillTargetRule SelfTriple => CreateRule(Side.Self, TargetRange.Triple);
    public static SkillTargetRule SelfAllRule => CreateRule(Side.Self, TargetRange.All);

    public static SkillTargetRule OpponentSingleRule => CreateRule(Side.Opponent, TargetRange.Single);
    public static SkillTargetRule OpponentDoubleRule => CreateRule(Side.Opponent, TargetRange.Double);
    public static SkillTargetRule OpponentAllRule => CreateRule(Side.Opponent, TargetRange.All);

    public static SkillTargetRule AllRule => CreateRule(Side.All, TargetRange.All);

    public static SlotData RedZeroSlot => CreateRedSlot(0);
    public static SlotData RedOneSlot => CreateRedSlot(1);
    public static SlotData BlueZeroSlot => CreateBlueSlot(0);
    public static SlotData BlueOneSlot => CreateBlueSlot(1);

    public static TraitConfig CreateTraitConfig(int chargeAttack = 0, float guardBonusRate = 0, float ampliyRate = 0, float breakRate = 0) 
        => new TraitConfig(chargeAttack, guardBonusRate, ampliyRate, breakRate);


    public static PhaseData CreatePhaseData(GamePhase phase, params Team[] order) => new PhaseData(phase, new Phase(order));
    public static PhaseFlowOrchestrator CreatePhaseManager(params PhaseData[] phaseDatas) => CreatePhaseManager(new PhaseEventDispatcher(), phaseDatas);
    public static PhaseFlowOrchestrator CreatePhaseManager(PhaseEventDispatcher eventDispatcher, params PhaseData[] phaseDatas) => new PhaseFlowOrchestrator(phaseDatas, eventDispatcher, new TeamPhaseEntryDispatcher(new TestEntry(), new TestEntry()));
    public static GameBanPickStorage CreateStorage(params int[] selectableIds) => new GameBanPickStorage(selectableIds);
    public static GameFlowData CreateFlow(GamePhase phase, Team turn) => new GameFlowData(phase, turn);
}

public class TestAttackChangeAction : ISkillAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}

public class TestEntry : IPhaseEntry
{
    public int Count = 0;

    public void EnterBan() => Count++;
    public void EnterPick() => Count += 2;
}
