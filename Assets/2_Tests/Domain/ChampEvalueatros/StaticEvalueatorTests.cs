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


    SkillEvaluator CreateEvaluator(int teamSize, ChampionStatData averageStat, SlotStorage<ChampionStatus> statusSlots) 
        => new SkillEvaluator(statusSlots, new StatTeamPredictor(new ChampionStatAverager(new ChampionStatData[] { averageStat }), teamSize));


    [Test]
    public void 평가_시_원본_값은_건드리면_안됨(Side side, int expected)
    {
        var skill = CreateConditionFreeExecutor(new TestAttackChangeAction(1000));
        var statusSlots = CreateTwoSlotStatus();
        var sut = CreateEvaluator(5, default, statusSlots);

        int result = sut.Evaluate(skill, Team.Blue, AllRule);

        Assert.AreEqual(0, statusSlots.GetSlot(BlueZeroSlot));
    }

    [Test]
    [TestCase(Side.Self, 500)]
    [TestCase(Side.Opponent, -500)]
    [TestCase(Side.All, 0)]
    public void 조건_없는_스킬은_값과_타겟_범위에_따라_평가(Side side, int expected)
    {
        var skill = CreateConditionFreeExecutor(new AttackChanger(100));
        var sut = CreateEvaluator(5, default, CreateTwoSlotStatus());

        int result = sut.Evaluate(skill, Team.Blue, new TraitTargetRule(side, TargetRange.All));

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void 조건_있는_스킬은_값과_유효한_타겟_범위에_따라_평가()
    {
        var skill = CreateSkillExecutor(new DefenseChanger(100), new StatThresholdChecker(StatConditionType.AttackBelow, 100));
        var sut = CreateEvaluator(5, CreateStat(), CreateTwoSlotStatus(att: 10000));

        int result = sut.Evaluate(skill, Team.Blue, SelfAllRule);

        Assert.AreEqual(200, result);
    }

    [Test]
    public void 고정은_변화값_스탯_변화_종합한_가치()
    {
        var skill = CreateConditionFreeExecutor(new DefenseFixer(100));
        var sut = CreateEvaluator(5, CreateStat(), CreateTwoSlotStatus(def:50));

        int result = sut.Evaluate(skill, Team.Blue, SelfAllRule);

        Assert.AreEqual(400, result);
    }
}
