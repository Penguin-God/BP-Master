using NUnit.Framework;
using static TestHelper;

public class SkillPreviewTests
{
    [Test]
    public void 현재_밴픽_상황과_스킬_주면_새로운_깊은복사한_객체에_적용후_반환()
    {
        var slots = CreateTwoSlotStatus();
        var sut = new SkillPreviewer(CreateSkillExceutorFactory(), slots);

        var result = sut.PreviewSkill(new Champion(1, CreateValueSkill(SkillType.AttackChanger, 100, rule: SelfAllRule), CreateStatus()), CreateBlueSlots(0, 1));

        Assert.AreEqual(0, slots.GetSlot(BlueZeroSlot).Stat.Attack); // 원본 그대로
        Assert.AreEqual(100, result.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(100, result.GetSlot(BlueOneSlot).Stat.Attack);
        Assert.AreEqual(0, result.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 스킬_적용_전후_점수차_계산()
    {
        var slots = CreateTwoSlotStatus(att:1000);
        var previewer = new SkillPreviewer(CreateSkillExceutorFactory(), slots);
        var sut = new PickScoreDeltaCalculator(previewer, slots);
        var champion = new Champion(1, CreateValueSkill(SkillType.AttackChanger, 100, rule: SelfAllRule), CreateStatus());

        GameStatChangeInfo result = sut.CalculateApplySkillStat(champion, CreateBlueSlots(0, 1));
        Assert.AreEqual(200, result.Blue.Att);

        champion = new Champion(1, CreateValueSkill(SkillType.AttackChanger, -100, rule: SelfAllRule), CreateStatus());
        result = sut.CalculateApplySkillStat(champion, CreateRedSlots(0, 1));
        Assert.AreEqual(-200, result.Red.Att);
    }
}
