using NUnit.Framework;
using static TestHelper;

public class SkillPreviewTests
{
    [Test]
    public void 현재_밴픽_상황과_스킬_주면_새로운_깊은복사한_객체에_적용후_반환()
    {
        var slots = CreateTwoSlotStatus();
        var result = SkillPreviewer.PreviewSkill(Team.Blue, CreateChampion(1, skillData: CreateValueSkillData(StatType.Attack, 100, rule: SelfAllRule)), slots);

        Assert.AreEqual(0, slots.GetSlot(BlueZeroSlot).Stat.Attack); // 원본 스탯은 그대로
        Assert.AreEqual(100, result.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(100, result.GetSlot(BlueOneSlot).Stat.Attack);
        Assert.AreEqual(0, result.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 카드_픽과_스킬_사용_시뮬()
    {
        var slots = CreateOneSlotStatus();
        var result = SkillPreviewer.SimulatePickCard(Team.Blue, CreateChampion(1, skillData: CreateValueSkillData(StatType.Attack, 100, rule: SelfAllRule)), slots);

        Assert.AreEqual(0, slots.GetSlot(BlueZeroSlot).Stat.Attack); // 원본 스탯은 그대로

        Assert.AreEqual(100, result.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(100, result.GetSlot(BlueOneSlot).Stat.Attack); // 새로운 카드 픽
    }

    [Test]
    public void 스탯_슬롯_전후_점수차_계산()
    {
        var originSlots = CreateTwoSlotStatus(att: 1000, def: 100, speed:5);
        var afterSlots = CreateTwoSlotStatus(att: 800);

        var result = ScoreDeltaCalculator.CalculateStatDelta(originSlots, afterSlots);
        Assert.AreEqual(-400, result.Red.Att);
        Assert.AreEqual(-200, result.Red.Def);
        Assert.AreEqual(-10, result.Red.Speed);

        Assert.AreEqual(-400, result.Blue.Att);
        Assert.AreEqual(-200, result.Blue.Def);
        Assert.AreEqual(-10, result.Blue.Speed);
    }

    [Test]
    public void 팀에_따라_아군_적군_합해서_밸류_계산()
    {
        var data = new GameScoreInfo(new ScoreInfo(100, 100, 5), new ScoreInfo(-500, 300, 0));

        int result = ScoreDeltaCalculator.CalculateScoreDelta(data, Team.Blue, 10);

        // (100 + 100 + 50) = 250
        // -500 +300 = -200을 상대팀이니까 +로 변환
        Assert.AreEqual(450, result);
    }
}
