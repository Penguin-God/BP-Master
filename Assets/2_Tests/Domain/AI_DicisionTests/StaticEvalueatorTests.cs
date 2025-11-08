using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class StaticEvalueatorTests
{
    [Test]
    [TestCase(1, 75)]
    [TestCase(2, 60)]
    public void 정적_가치는_공방과_숙련도의_합(int champId, int excpected)
    {
        IEnumerable<ChampionMastery> masteries = new ChampionMastery[] { new ChampionMastery(1, 15) };
        var sut = new StaticValueEvaluator(masteries);
        ChampionStatData stat = CreateStat(att: 20, def: 40);

        int result = sut.Evaluate(champId, stat);

        Assert.AreEqual(excpected, result);
    }


    SkillEvaluator CreateEvaluator(int teamSize, SlotStorage<ChampionStatus> statusSlots) => new SkillEvaluator(statusSlots, teamSize);


    [Test]
    [TestCase(Side.Self, 500)]
    [TestCase(Side.Opponent, -500)]
    [TestCase(Side.All, 0)]
    public void 조건_없는_스킬은_값과_타겟_범위에_따라_평가(Side side, int expected)
    {
        var skill = CreateSkillData(SkillType.AttackChanger, 100, default, new TraitTargetRule(side, TargetRange.All));
        var sut = CreateEvaluator(5, CreateTwoSlotStatus());

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(Side.Self, 250)]
    [TestCase(Side.Opponent, -200)]
    [TestCase(Side.All, 50)]
    public void 조건_스킬은_이미_픽한_경우_검사하지만_빈_슬롯은_값의_절반으로_계산(Side side, int expected)
    {
        var skill = CreateSkillData(SkillType.DefenseChanger, 100, CreateThresholdCondition(StatConditionType.AttackAtLeast, 100), new TraitTargetRule(side, TargetRange.All));
        var statusSlots = CreateOneSlotStatus();
        var sut = CreateEvaluator(5, statusSlots);
        statusSlots.AddSlot(Team.Blue, CreateStatus(att: 100));

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(expected, result);
    }
}
