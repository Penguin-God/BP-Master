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
    public void 잘못된_타겟은_null()
    {
        var sut = new TraitTargetSelector(3);

        Assert.IsNull(sut.GetTargetSlot(Team.Blue, Side.Opponent, TargetRange.Single, CreateSlot(Team.Blue, 0)));
        Assert.IsNull(sut.GetTargetSlot(Team.Blue, Side.Self, TargetRange.All, CreateSlot(Team.Red, 0)));
        Assert.IsNull(sut.GetTargetSlot(Team.Red, Side.Self, TargetRange.Single, CreateSlot(Team.Blue, 0)));
    }

    [Test]
    public void 싱글은_단일_슬롯_반환()
    {
        var sut = new TraitTargetSelector(3);

        Assert.AreEqual(CreateSlot(Team.Red, 0), sut.GetTargetSlot(Team.Blue, Side.Opponent, TargetRange.Single, CreateSlot(Team.Red, 0)).First());
        Assert.AreEqual(CreateSlot(Team.Blue, 1), sut.GetTargetSlot(Team.Red, Side.Opponent, TargetRange.Single, CreateSlot(Team.Blue, 1)).First());
        Assert.AreEqual(CreateSlot(Team.Blue, 2), sut.GetTargetSlot(Team.Blue, Side.Self, TargetRange.Single, CreateSlot(Team.Blue, 2)).First());
    }

    [Test]
    public void All은_전체_슬롯_반환()
    {
        var sut = new TraitTargetSelector(3);

        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Red, 0), CreateSlot(Team.Red, 1), CreateSlot(Team.Red, 2)), sut.GetTargetSlot(Team.Blue, Side.Opponent, TargetRange.All, CreateSlot(Team.Red, 0)));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Red, 0), CreateSlot(Team.Red, 1), CreateSlot(Team.Red, 2)), sut.GetTargetSlot(Team.Blue, Side.Opponent, TargetRange.All, CreateSlot(Team.Red, 1)));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), sut.GetTargetSlot(Team.Blue, Side.Self, TargetRange.All, CreateSlot(Team.Blue, 1)));
        CollectionAssert.AreEqual(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), sut.GetTargetSlot(Team.Red, Side.Opponent, TargetRange.All, CreateSlot(Team.Blue, 2)));
    }

    ChampionSlot CreateSlot(Team team, int index) => new ChampionSlot(team, index);
    ChampionSlot[] CreateSlots(params ChampionSlot[] slots) => slots;
}
