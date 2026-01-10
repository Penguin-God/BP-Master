using NUnit.Framework;
using static TestHelper;

public class StaticEvalueatorTests
{
    [Test]
    public void 속도_가치에_따라_스탯_총합_계산()
    {
        var sut = new ChampionStatValueCalculator(10);
        ChampionStatData stat = CreateStat(att: 20, def: 40, speed:5);

        int result = sut.CalculateStatValue(stat);

        Assert.AreEqual(110, result);
    }

    [Test]
    public void 팀에_따라_아군_적군_합해서_밸류_계산()
    {
        var sut = new ChampionStatValueCalculator(10);
        var data = new GameStatChangeInfo(new TeamStatChangeInfo(100, 100, 5), new TeamStatChangeInfo(-500, 300, 0));

        int result = sut.CalcualteTeamStatValue(data, Team.Blue);

        // 250 + 200 = 450
        Assert.AreEqual(450, result);
    }

    SkillEvaluator CreateEvaluator(int teamSize, SlotStorage<ChampionStatus> statusSlots) => new SkillEvaluator(statusSlots, teamSize);


    [Test]
    [TestCase(Side.Self, 500)]
    [TestCase(Side.Opponent, -500)]
    [TestCase(Side.All, 0)]
    public void 조건_없는_스킬은_값과_타겟_범위에_따라_평가(Side side, int expected)
    {
        var skill = CreateValueSkillData(SkillType.AttackChanger, 100, default, new SkillTargetRule(side, TargetRange.All));
        var sut = CreateEvaluator(5, CreateTwoSlotStatus());

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(Side.Self, 250)]
    [TestCase(Side.Opponent, -200)]
    [TestCase(Side.All, 50)]
    public void 조건_스킬은_검사_후_계산(Side side, int expected)
    {
        var skill = CreateValueSkillData(SkillType.DefenseChanger, 100, CreateThresholdCondition(StatConditionType.AttackAtLeast, 100), new SkillTargetRule(side, TargetRange.All));
        var statusSlots = CreateOneSlotStatus();
        var sut = CreateEvaluator(5, statusSlots);
        statusSlots.AddSlot(Team.Blue, CreateStatus(att: 100));

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(expected, result);
    }
}
