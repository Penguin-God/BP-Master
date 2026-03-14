using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateSlot(Team team, int index) => new SlotData(team, index);
    public static SlotData CreateBlueSlot(int index) => CreateSlot(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => CreateSlot(Team.Red, index);

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

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(index => CreateBlueSlot(index));
    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(index => CreateRedSlot(index));

    public static ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0) => new ChampionStatus(CreateStat(att, def, speed));
    public static Skill CreateSkill(params SkillData[] skills) => new Skill(skills);
    public static SkillData[] CreateSkillDatas(params SkillData[] skills) => skills;

    public static SkillData CreateConditionFreeSkill(StatType statType, int amount, SkillTargetRule rule = default) => CreateValueSkillData(statType, amount, default, rule);

    public static SkillData CreateValueSkillData(StatType statType, int value, SkillConditionData conditionData = default, SkillTargetRule rule = default)
        => new SkillData(SkillType.StatChanger, CreateSkillAmount(AmountType.Value, statType, value: value), conditionData, rule);

    public static SkillData CreateAttackChangeSkill(int value, SkillConditionData conditionData = default, SkillTargetRule rule = default) => CreateValueSkillData(StatType.Attack, value, conditionData, rule);

    public static SkillAmountData CreateSkillAmount(AmountType amountType, StatType statType = StatType.Attack, int value = 0, float percent = 0, int fix = 0)
        => new SkillAmountData(amountType, statType, value, percent, fix);

    public static Skill CreateValueSkill(StatType statType, int amount, SkillConditionData conditionData = default, SkillTargetRule rule = default)
        => new Skill(CreateSkillDatas(CreateValueSkillData(statType, amount, conditionData, rule)));

    public static SkillConditionData CreateThresholdCondition(StatConditionType type, int threshold) => CreateConditionData(ConditionType.Threshold, statType: type, threshold: threshold);
    public static SkillConditionData CreateCompareCondition(StatConditionType type) => CreateConditionData(ConditionType.Compare, statType: type);
    public static SkillConditionData CreateConditionData(ConditionType conditionType, StatConditionType statType = StatConditionType.None, int threshold = 0) => new SkillConditionData(statType, threshold, conditionType);

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

    public static PhaseData CreatePhaseData(GamePhase phase, params Team[] order) => new PhaseData(phase, new Phase(order));
    public static PhaseFlowOrchestrator CreatePhaseManager(params PhaseData[] phaseDatas) => CreatePhaseManager(new PhaseEventDispatcher(), phaseDatas);
    public static PhaseFlowOrchestrator CreatePhaseManager(PhaseEventDispatcher eventDispatcher, params PhaseData[] phaseDatas) => new PhaseFlowOrchestrator(CreatePhaseAdvancer(phaseDatas), eventDispatcher, new TeamPhaseEntryDispatcher(new TestEntry(), new TestEntry()));
    public static PhaseAdvancer CreatePhaseAdvancer(params PhaseData[] phaseDatas) => new PhaseAdvancer(phaseDatas);
    public static BanPickStorage CreateStorage(params int[] selectableIds) => new BanPickStorage(selectableIds);
    public static GameFlowData CreateFlow(GamePhase phase, Team turn) => new GameFlowData(phase, turn);

    public static SkillActionFactory CreateSkillActionFactory() => new SkillActionFactory(new BanPickEventDispatcher(), new PhaseEventDispatcher());
    public static SkillExecutor CreateAttackChangeExecutor(int value) => new SkillExecutor(new TestAttackChangeAction(value), new NullChecker());
    public static SkillRunner CreateSkillRunner() => new SkillRunner(CreateSkillActionFactory(), new SkillCondtionFactory());

    public static Champion CreateChampion(int id = 0, int att = 0, int def = 0, int speed = 0, params SkillData[] skillData) => new Champion(id, new Skill(skillData), CreateStatus(att, def, speed));
    public static ChampionCatalog CreateCaltalog(params Champion[] champions) => new ChampionCatalog(champions);

    public static BonusCalculator CreateBonus(int needScore, int bonus) => new BonusCalculator(new SortedDictionary<int, int>() { { needScore, bonus } });

    public static ChampionMastery CreateMasteryData(int id, int att = 0, int def = 0, int speed = 0) => new ChampionMastery(id, CreateStat(att, def, speed));
    public static MasteryApplier CreateMasteryApplier(params ChampionMastery[] masteries) => new MasteryApplier(new MasteryCollection(masteries));
    public static MasteryProfile CreateMasteryInventory(int point, params int[] ids) => new MasteryProfile(ids, point);
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

    readonly int BanCount;
    readonly int PickCount;
    public TestEntry(int banCount = 0, int pickCount = 0)
    {
        BanCount = banCount;
        PickCount = pickCount;
    }

    public void EnterBan() => Count += BanCount;
    public void EnterPick() => Count += PickCount;
}
