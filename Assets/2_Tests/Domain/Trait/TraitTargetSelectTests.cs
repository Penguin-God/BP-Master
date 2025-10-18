using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class TraitTargetSelectTests
{
    int[] AllIndexs = new int[] { 0, 1, 2, 3, 4 };
    TraitTargetRule CreateDoubleRule(Side side) => new TraitTargetRule(side, TargetRange.Double);

    [Test]
    public void All은_선택_시_전체_세팅()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, AllRule);

        sut.Select(BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(AllIndexs).Concat(CreateRedSlots(AllIndexs)), sut.Targets);
    }

    [Test]
    public void 특정_사이드_All은_선택한_팀_전체_세팅()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, new TraitTargetRule(Side.Opponent, TargetRange.All));

        sut.Select(BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(AllIndexs), sut.Targets);
    }

    [Test]
    public void 같은_슬롯은_저장_안됨()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, CreateDoubleRule(Side.All));

        sut.Select(BlueZeroSlot);
        sut.Select(BlueZeroSlot);

        Assert.AreEqual(1, sut.Targets.Count());
    }

    [Test]
    public void Type에_따른_개수_저장()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, CreateDoubleRule(Side.All));

        sut.Select(BlueZeroSlot);
        Assert.IsFalse(sut.IsFull);
        sut.Select(BlueOneSlot);

        Assert.IsTrue(sut.IsFull);
        Assert.AreEqual(2, sut.Targets.Count());
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), sut.Targets);
    }
}
