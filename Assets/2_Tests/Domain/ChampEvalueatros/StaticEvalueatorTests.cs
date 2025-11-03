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
        ChampionStatData stat = TestHelper.CreateStat(att: 20, def: 40);

        int result = sut.Evaluate(champId, stat);

        Assert.AreEqual(excpected, result);
    }

    [Test]
    public void 조건_없는_스킬_가치는_값과_팀_수의_곱()
    {
        SkillData skill = new SkillData(SkillType.AttackChanger, 50, default, OpponentAllRule);
        SlotStorage<ChampionStatus> statuses = CreateTwoSlotStatus();
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill);

        Assert.AreEqual(100, result);
    }
}
