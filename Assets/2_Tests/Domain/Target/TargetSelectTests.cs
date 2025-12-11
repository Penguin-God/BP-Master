using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class SkillTargetSelectTests
{
    SkillTargetRule CreateDoubleRule(Side side) => new SkillTargetRule(side, TargetRange.Double);
    void Select(SkillTargetSelector sut, SlotData slot) => sut.Select(slot);
    SkillTargetSelector CreateSut(Team team, int blueSize, int redSize, SkillTargetRule rule) => new SkillTargetSelector(team, new SkillTargetCounter(blueSize, redSize), rule);

    [Test]
    public void 전체_All은_전부_넣음()
    {
        var sut = CreateSut(Team.Blue, blueSize: 2, redSize : 3, AllRule);

        Select(sut, BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1).Concat(CreateRedSlots(0, 1, 2)), sut.Targets);
    }

    [Test]
    public void 특정_사이드_All은_선택한_팀_전체_세팅()
    {
        var sut = CreateSut(Team.Blue, blueSize:0, redSize:4, OpponentAllRule);

        Select(sut, RedOneSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2, 3), sut.Targets);
    }

    [Test]
    public void 중복_저장_안됨()
    {
        var sut = CreateSut(Team.Blue, 5, 5, CreateDoubleRule(Side.All));

        Select(sut, BlueZeroSlot);
        Select(sut, BlueZeroSlot);

        Assert.AreEqual(1, sut.Targets.Count());
    }

    [Test]
    public void 타겟_수_초과되면_무시()
    {
        var sut = CreateSut(Team.Blue, 5, 5, OpponentSingleRule);

        Select(sut, BlueZeroSlot);
        Select(sut, BlueOneSlot);
        Assert.AreEqual(1, sut.Targets.Count());
    }

    [Test]
    public void 타겟_수가_팀보다_크면_팀을_다_고르면_Full()
    {
        var sut = CreateSut(Team.Blue, 1, 1, OpponentDoubleRule);

        Select(sut, BlueZeroSlot);
        Assert.IsTrue(sut.IsFull);
    }

    [Test]
    public void 팀_수가_0이면_Full()
    {
        var sut = CreateSut(Team.Blue, 0, 0, SelfTriple);
        Assert.IsTrue(sut.IsFull);
    }
}
