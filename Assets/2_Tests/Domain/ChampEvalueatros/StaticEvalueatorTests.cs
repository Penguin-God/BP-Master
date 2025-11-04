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

    [Test]
    [TestCase(Side.Self, 100)]
    [TestCase(Side.Opponent, -100)]
    [TestCase(Side.All, 0)]
    public void 조건_없는_스킬_가치는_값과_타겟_범위에_따라_평가(Side side, int expected)
    {
        var skill = CreateNullCkeckSkill(SkillType.AttackChanger, 50, new TraitTargetRule(side, TargetRange.All));
        SlotStorage<ChampionStatus> statuses = CreateTwoSlotStatus();
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill);

        Assert.AreEqual(expected, result);
    }
    SkillData CreateNullCkeckSkill(SkillType type, int amount, TraitTargetRule rule) => new SkillData(type, amount, default, rule);
}
