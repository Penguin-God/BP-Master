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
    public void 조건_없는_스킬_가치는_값과_타겟_수의_곱()
    {
        var skill = CreateNullCkeckSkill(SkillType.AttackChanger, 50, SelfAllRule);
        SlotStorage<ChampionStatus> statuses = CreateTwoSlotStatus();
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill);

        Assert.AreEqual(100, result);
    }

    [Test]
    public void 상대방의_스탯변화_평가는_부호_반대로()
    {
        var skill = CreateNullCkeckSkill(SkillType.AttackChanger, 50, OpponentAllRule);
        SlotStorage<ChampionStatus> statuses = CreateTwoSlotStatus();
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill);

        Assert.AreEqual(-100, result);
    }

    SkillData CreateNullCkeckSkill(SkillType type, int amount, TraitTargetRule rule) => new SkillData(type, amount, default, rule);
}
