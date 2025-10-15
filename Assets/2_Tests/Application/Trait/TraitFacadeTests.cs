using NUnit.Framework;
using static TestHelper;

public class TraitFacadeTests
{
    [Test]
    public void 특성_컬랙션_적용()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));
        TraitData[] datas = new TraitData[] { CreateAttTraitData(10, range: TargetRange.Single), TestHelper.CreateTraitData(TraitType.DefenseChanger, 10, side: Side.Self, range: TargetRange.Single) };
        var sut = new TraitUseFacade(statuses);

        sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), datas);

        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Defense);
    }
}
