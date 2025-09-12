using NUnit.Framework;
using System.Linq;

public class TraitTargetFindingTests
{
    [Test]
    public void 싱글은_단일_index_반환()
    {
        var sut = new TraitTargetSelector(3);

        Assert.AreEqual(1, sut.GetTargetIds(TargetRange.Single, 1).First());
        Assert.AreEqual(2, sut.GetTargetIds(TargetRange.Single, 2).First());
    }

    [Test]
    public void All은_전체_index_반환()
    {
        var sut = new TraitTargetSelector(3);

        CollectionAssert.AreEqual(new int[] { 0, 1, 2 }, sut.GetTargetIds(TargetRange.All, 0));
    }

    [Test]
    public void 적절한_팀_전체_반환()
    {
        var sut = new TraitTargetSelector(3);

        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Red, 0), CreateSlot(Team.Red, 1), CreateSlot(Team.Red, 2)), sut.GetTargetableSlot(Team.Blue, Side.Opponent));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), sut.GetTargetableSlot(Team.Red, Side.Opponent));
    }

    ChampionSlot CreateSlot(Team team, int index) => new ChampionSlot(team, index);
    ChampionSlot[] CreateSlots(params ChampionSlot[] slots) => slots;
}
