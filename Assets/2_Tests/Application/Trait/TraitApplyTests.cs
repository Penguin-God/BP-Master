using NUnit.Framework;
using static TestHelper;

public class TraitApplyTests
{
    SlotStorage<ChampionStatus> CreateOneSlotStatus(int att = 0, int def = 0, int speed = 0)
    {
        SlotStorage<ChampionStatus> result = new();
        result.AddSlot(Team.Blue, CreateStatus(att, def, speed));
        result.AddSlot(Team.Red, CreateStatus(att, def, speed));
        return result;
    }

    [Test]
    public void 특성_사용후_플래그가_바뀌며_다음_실행_조건은_바뀐_값을_반영함()
    {
        var statuses = CreateOneSlotStatus();

        var condition = CreateThresholdCondition(TraitConditionType.AttackBelow, 10);
        var traitData = CreateTraitData(TraitType.AttackChanger, 15, condition, OpponentAllRule);
        TraitApplier sut = new TraitApplier(statuses);

        sut.Execute(traitData, CreateRedSlot(0), CreateBlueSlot(0));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.IsTrue(sut.IsUse);

        // 아까 실행으로 조건이 거짓이 되었으므로 아무일도 안생김
        sut.Execute(traitData, CreateRedSlot(0), CreateBlueSlot(0));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }

    [Test]
    public void 특성_전원_적용()
    {
        var statuses = CreateOneSlotStatus();

        var traitData = CreateConditionFreeTrait(TraitType.AttackChanger, 15, AllRule);
        TraitApplier sut = new TraitApplier(statuses);

        sut.Execute(traitData, CreateRedSlot(0), CreateBlueSlot(0));

        Assert.AreEqual(15, statuses.GetSlot(CreateBlueSlot(0)).Stat.Attack);
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }

    [Test]
    [TestCase(100, 0)]
    [TestCase(0, 10)]
    public void 서로_비교하는_조건에_따른_실행(int att, int expected)
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus(att)); // 레드 초과의 값이여야 실행됨
        statuses.AddSlot(Team.Red, CreateStatus(10));

        var condition = CreateCompareCondition(TraitConditionType.AttackBelow);
        var traitData = CreateTraitData(TraitType.AttackChanger, -10, condition, OpponentAllRule);
        TraitApplier sut = new TraitApplier(statuses);

        sut.Execute(traitData, CreateRedSlot(0), CreateBlueSlot(0));

        Assert.AreEqual(expected, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }
}
