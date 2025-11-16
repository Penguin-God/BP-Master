using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class TraitTargetSelectTests
{
    int[] AllIndexs = new int[] { 0, 1, 2, 3, 4 };
    SkillTargetRule CreateDoubleRule(Side side) => new SkillTargetRule(side, TargetRange.Double);
    void Select(TraitTargetSelector sut, SlotData slot) => sut.Select(slot);

    [Test]
    public void All은_선택_시_전체_세팅()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, AllRule);

        Select(sut, BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(AllIndexs).Concat(CreateRedSlots(AllIndexs)), sut.Targets);
    }

    [Test]
    public void 특정_사이드_All은_선택한_팀_전체_세팅()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, new SkillTargetRule(Side.Opponent, TargetRange.All));

        Select(sut, BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(AllIndexs), sut.Targets);
    }

    [Test]
    public void 같은_슬롯은_저장_안됨()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, CreateDoubleRule(Side.All));

        Select(sut, BlueZeroSlot);
        Select(sut, BlueZeroSlot);

        Assert.AreEqual(1, sut.Targets.Count());
    }

    [Test]
    public void 타겟_수_초과되면_무시()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, OpponentSingleRule);

        Select(sut, BlueZeroSlot);
        Assert.AreEqual(1, sut.Targets.Count());

        Select(sut, BlueZeroSlot);
        Assert.AreEqual(1, sut.Targets.Count());
    }

    [Test]
    public void Type에_따른_개수_저장()
    {
        TraitTargetSelector sut = new TraitTargetSelector(5, CreateDoubleRule(Side.All));

        Select(sut, BlueZeroSlot);
        Assert.IsFalse(sut.IsFull);
        Select(sut, BlueOneSlot);

        Assert.IsTrue(sut.IsFull);
        Assert.AreEqual(2, sut.Targets.Count());
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), sut.Targets);
    }
}
