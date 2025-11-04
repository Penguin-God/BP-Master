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

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void 조건_있는_스킬_가치는_값과_유효한_타겟_범위에_따라_평가()
    {
        var skill = new SkillData(SkillType.DefenseChanger, 50, CreateThresholdCondition(StatConditionType.AttackBelow, 100) ,SelfAllRule);
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        statuses.AddSlot(Team.Blue, CreateStatus(att: 1000));
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(50, result);
    }

    [Test]
    public void 고정은_변화값_스탯_변화_종합한_가치()
    {
        var skill = CreateNullCkeckSkill(SkillType.DefenseChanger, 50, SelfAllRule);
        SlotStorage<ChampionStatus> statuses = CreateTwoSlotStatus(def:50);
        var sut = new SkillEvaluator(statuses);

        int result = sut.Evaluate(skill, Team.Blue);

        Assert.AreEqual(100, result);
    }

    [Test]
    public void 챔피언의_평균_스탯_반환()
    {
        var stats = new ChampionStatData[] { CreateStat(0, 0), CreateStat(100, 100) };
        var sut = new ChampionStatAverager(stats);

        ChampionStatData result = sut.GetStatAverage();


        Assert.AreEqual(50, result.Attack);
        Assert.AreEqual(50, result.Defense);
    }

    SkillData CreateNullCkeckSkill(SkillType type, int amount, TraitTargetRule rule) => new SkillData(type, amount, default, rule);
}
