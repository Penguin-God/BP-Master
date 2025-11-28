using NUnit.Framework;
using static TestHelper;

public class SkillPreviewTests
{
    [Test]
    public void 현재_밴픽_상황과_스킬_주면_새로운_깊은복사한_객체에_적용후_반환()
    {
        var slots = CreateOneSlotStatus();
        var sut = new SkillPreviewer();

        var result = sut.PreviewSkill(slots, CreateValueSkill(SkillType.AttackChanger, 100, rule: SelfAllRule));

        Assert.AreEqual(0, slots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(100, result.GetSlot(BlueZeroSlot).Stat.Attack);
    }
}
