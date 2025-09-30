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
        sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateAttTraitData(10, range: TargetRange.Single));
        Assert.IsTrue(sut.IsTraitUsed(CreateBlueSlot(0)));
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);

        // 사용 후 실행 X
        sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), null);
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }
}
