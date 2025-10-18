using NUnit.Framework;
using static TestHelper;

public class TraitApplyTests
{
    TraitApplier CreateSut(SlotStorage<ChampionStatus> statuses, SlotData slot) => new TraitApplier(statuses, slot);

    [Test]
    public void 특성_사용후_다음_실행은_바뀐_값을_반영해_조건_검사()
    {
        var statuses = CreateOneSlotStatus();

        var condition = CreateThresholdCondition(TraitConditionType.AttackBelow, 10);
        var traitData = CreateTraitData(TraitType.AttackChanger, 15, condition, OpponentAllRule);
        var sut = CreateSut(statuses, BlueZeroSlot);

        sut.Execute(traitData, RedZeroSlot);
        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        
        // 아까 실행으로 조건이 거짓이 되었으므로 아무일도 안생김
        sut.Execute(traitData, RedZeroSlot);
        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 특성_사용후_플래그_바뀌고_이벤트_알림()
    {
        var statuses = CreateOneSlotStatus();
        var traitData = CreateConditionFreeTrait(TraitType.AttackChanger, 15, OpponentAllRule);
        var callSlot = RedZeroSlot;

        var sut = CreateSut(statuses, BlueZeroSlot);
        sut.OnUseTrait += slot => callSlot = slot;

        sut.Execute(traitData, RedZeroSlot);


        Assert.IsTrue(sut.IsUse);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }

    [Test]
    public void 특성_전원_적용()
    {
        var statuses = CreateOneSlotStatus();

        var traitData = CreateConditionFreeTrait(TraitType.AttackChanger, 15, AllRule);
        var sut = CreateSut(statuses, BlueZeroSlot);

        sut.Execute(traitData, RedZeroSlot);

        Assert.AreEqual(15, statuses.GetSlot(CreateBlueSlot(0)).Stat.Attack);
        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
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
        var sut = CreateSut(statuses, BlueZeroSlot);

        sut.Execute(traitData, RedZeroSlot);

        Assert.AreEqual(expected, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
