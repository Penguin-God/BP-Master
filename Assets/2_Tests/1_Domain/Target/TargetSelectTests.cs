using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static TestHelper;

public class SkillTargetSelectTests
{
    SkillTargetRule CreateDoubleRule(Side side) => new SkillTargetRule(side, TargetRange.Double);
    TargetSelection Select(TargetSelection sut, SlotData slot) => TargetSelectionRules.Select(sut, slot);
    TargetSelection CreateSut(Team team, int blueSize, int redSize, SkillTargetRule rule) => new TargetSelection(new SlotData(team, 0), new TargetFindContext(team, rule, new TeamCount(blueSize, redSize)), new HashSet<SlotData>());

    [Test]
    public void 필드_전체_룰은_모든_요소_반환()
    {
        var sut = CreateSut(Team.Blue, 2, 3, AllRule);

        sut = Select(sut, BlueZeroSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1).Concat(CreateRedSlots(0, 1, 2)), sut.Selected);
    }

    [Test]
    public void 특정_팀_All은_선택한_팀_전체_세팅()
    {
        var sut = CreateSut(Team.Blue, 0, 4, OpponentAllRule);

        sut = Select(sut, RedOneSlot);

        Assert.IsTrue(sut.IsFull);
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2, 3), sut.Selected);
    }

    [Test]
    public void All이_아니면_차곡차곡_저장()
    {
        var sut = CreateSut(Team.Blue, 5, 5, SelfTriple);

        sut = Select(sut, BlueZeroSlot);
        sut = Select(sut, BlueOneSlot);

        Assert.AreEqual(2, sut.Selected.Count);
    }

    [Test]
    public void 중복_저장_안됨()
    {
        var sut = CreateSut(Team.Blue, 5, 5, CreateDoubleRule(Side.All));

        sut = Select(sut, BlueZeroSlot);
        sut = Select(sut, BlueZeroSlot);

        Assert.AreEqual(1, sut.Selected.Count);
    }

    [Test]
    public void 타겟_수_초과되면_Full_플래그가_켜지고_추가_무시()
    {
        var sut = CreateSut(Team.Blue, 5, 5, OpponentSingleRule);

        sut = Select(sut, BlueZeroSlot);

        Assert.AreEqual(1, sut.Selected.Count);
        Assert.IsTrue(sut.IsFull);

        sut = Select(sut, BlueOneSlot);
        Assert.AreEqual(1, sut.Selected.Count);
    }

    [Test]
    public void 팀_수가_0이면_Full()
    {
        var sut = CreateSut(Team.Blue, 0, 0, SelfTriple);

        Assert.IsTrue(sut.IsFull);
    }

    [Test]
    public void 이미_선택된_슬롯은_선택_가능_목록에서_제외된다()
    {
        var sut = CreateSut(Team.Blue, 3, 0, SelfTriple);
        var selectedSlot = new SlotData(Team.Blue, 0);

        Assert.AreEqual(3, sut.GetSelectableTargets().ToArray().Length);

        sut = Select(sut, selectedSlot);
        var result = sut.GetSelectableTargets().ToArray();

        Assert.AreEqual(2, result.Length);
        Assert.IsFalse(result.Contains(selectedSlot));
    }
}
