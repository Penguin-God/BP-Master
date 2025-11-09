using NUnit.Framework;
using static TestHelper;

public class SkillUsePresenterTests
{
    [Test]
    public void 스킬_선택_후_타겟_가득_차면_참_반환()
    {
        var rule = new TraitTargetRule(Side.Opponent, TargetRange.Double);
        var sut = new SkillUsePersenter(teamSize: 2);

        SlotData useSlot = BlueZeroSlot;
        sut.SelectUseSkill(useSlot, rule);
        Assert.IsTrue(sut.IsUseable);

        Assert.IsFalse(sut.SelectTarget(RedZeroSlot, out useSlot));
        Assert.IsTrue( sut.SelectTarget(RedOneSlot, out useSlot));
        Assert.IsFalse(sut.IsUseable);

        Assert.AreEqual(BlueZeroSlot, useSlot);
    }
}