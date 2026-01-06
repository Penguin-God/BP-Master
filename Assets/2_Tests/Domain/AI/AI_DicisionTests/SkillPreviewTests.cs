using NUnit.Framework;
using static TestHelper;

public class SkillPreviewTests
{
    [Test]
    public void 현재_밴픽_상황과_스킬_주면_새로운_깊은복사한_객체에_적용후_반환()
    {
        var slots = CreateTwoSlotStatus();
        var sut = new SkillPreviewer(Team.Blue, CreateSkillExceutorFactory(), slots);

        var result = sut.PreviewSkill(Team.Blue, CreateChampion(1, skillData: CreateValueSkillData(SkillType.AttackChanger, 100, rule: SelfAllRule)));

        Assert.AreEqual(0, slots.GetSlot(BlueZeroSlot).Stat.Attack); // 원본 스탯은 그대로
        Assert.AreEqual(100, result.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(100, result.GetSlot(BlueOneSlot).Stat.Attack);
        Assert.AreEqual(0, result.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 스킬_적용_전후_점수차_계산()
    {
        var slots = CreateTwoSlotStatus(att:1000);
        var previewer = new SkillPreviewer(Team.Blue, CreateSkillExceutorFactory(), slots);
        var sut = new SkillApplyDeltaCalculator(previewer, slots);
        var champion = new Champion(1, CreateValueSkill(SkillType.AttackChanger, 100, rule: SelfAllRule), CreateStatus());

        GameStatChangeInfo result = sut.CalculateApplySkillStat(champion);
        Assert.AreEqual(200, result.Blue.Att);

        champion = new Champion(1, CreateValueSkill(SkillType.AttackChanger, -100, rule: OpponentAllRule), CreateStatus());
        result = sut.CalculateApplySkillStat(champion);
        Assert.AreEqual(-200, result.Red.Att);
    }

    [Test]
    public void 스탯_슬롯_전후_점수차_계산()
    {
        var originSlots = CreateTwoSlotStatus(att: 1000, def: 100, speed:5);
        var afterSlots = CreateTwoSlotStatus(att: 800);
        var sut = new SkillApplyDeltaCalculator();

        var result = sut.CalculateStatDelta(originSlots, afterSlots);
        Assert.AreEqual(-400, result.Red.Att);
        Assert.AreEqual(-200, result.Red.Def);
        Assert.AreEqual(-10, result.Red.Speed);

        Assert.AreEqual(-400, result.Blue.Att);
        Assert.AreEqual(-200, result.Blue.Def);
        Assert.AreEqual(-10, result.Blue.Speed);
    }
}
