using NUnit.Framework;
using static TestHelper;

public class TraitApplyTests
{
    TraitApplier CreateSut(SlotStorage<ChampionStatus> statuses, SlotData slot) => new TraitApplier(statuses, slot);
    void ExecuteSut(TraitApplier sut, TraitData trait, params SlotData[] slots) => sut.Execute(trait, slots); 
    [Test]
    public void 특성_사용후_다음_실행은_바뀐_값을_반영해_조건_검사()
    {
        var statuses = CreateOneSlotStatus();

        var condition = CreateThresholdCondition(TraitConditionType.AttackBelow, 10);
        var traitData = CreateTraitData(TraitType.AttackChanger, 15, condition);
        var sut = CreateSut(statuses, BlueZeroSlot);

        ExecuteSut(sut, traitData, RedZeroSlot);
        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
        
        // 아까 실행으로 조건이 거짓이 되었으므로 아무일도 안생김
        ExecuteSut(sut, traitData, RedZeroSlot);
        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 특성_사용후_플래그_바뀌고_이벤트_알림()
    {
        var statuses = CreateOneSlotStatus();
        var traitData = CreateConditionFreeTrait(TraitType.AttackChanger, 15);
        var callSlot = RedZeroSlot;

        var sut = CreateSut(statuses, BlueZeroSlot);
        sut.OnUseTrait += slot => callSlot = slot;

        ExecuteSut(sut, traitData, RedZeroSlot);

        Assert.IsTrue(sut.IsUse);
        Assert.AreEqual(BlueZeroSlot, callSlot);
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
        var traitData = CreateTraitData(TraitType.AttackChanger, -10, condition);
        var sut = CreateSut(statuses, BlueZeroSlot);

        ExecuteSut(sut, traitData, RedZeroSlot);

        Assert.AreEqual(expected, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 타겟들을_받아서_실행()
    {
        var statuses = CreateOneSlotStatus();
        var traitData = CreateConditionFreeTrait(TraitType.AttackChanger, 15);
        var sut = CreateSut(statuses, BlueZeroSlot);

        ExecuteSut(sut, traitData, RedZeroSlot);

        Assert.AreEqual(15, statuses.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
