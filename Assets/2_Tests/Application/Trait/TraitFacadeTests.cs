using NUnit.Framework;
using static TestHelper;

public class TraitFacadeTests
{
    [Test]
    public void 특성_사용_후_플래그는_참이되고_중복_사용_불가()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        var sut = new TraitUseFacade(statuses);

        Assert.IsFalse(sut.IsTraitUsed(CreateBlueSlot(0))); // 사용 전에는 false
        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 10), TargetRange.Single));
        Assert.IsTrue(sut.IsTraitUsed(CreateBlueSlot(0)));
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);

        Assert.IsFalse(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), null, TargetRange.All));
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }

    [Test]
    public void 조건은_실시간_반영()
    {
        SlotStorage<ChampionStatus> statuses = new();
        // Blue 2, Red 2 상태 초기화 (공격력 0)
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        TraitUseFacade sut = new TraitUseFacade(statuses);

        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 15, TraitConditionType.AttackBelow, 10), TargetRange.All));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(1)).Stat.Attack);

        // 사용은 되지만 조건이 안되서 적용 안됨
        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(1), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 15, TraitConditionType.AttackBelow, 10), TargetRange.All));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(1)).Stat.Attack);
    }
}
