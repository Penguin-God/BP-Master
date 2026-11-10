using NUnit.Framework;

public class SkillConditionTests
{
    [Test]
    public void None은_무조건_참() => Assert.IsTrue(Check(StatConditionType.None, 0, default));

    [Test]
    public void NullChecker은_무조건_참() => Assert.IsTrue(new NullChecker().Check(TestHelper.CreateStatus(), TestHelper.CreateStatus()));

    [Test]
    [TestCase(StatConditionType.AttackBelow)]
    [TestCase(StatConditionType.DefenseBelow)]
    public void 스탯이_기준_이하면_참(StatConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, default));
        Assert.IsTrue(Check(type, threshold, Stat(100, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(120, 120)));
    }

    [Test]
    [TestCase(StatConditionType.AttackAtLeast)]
    [TestCase(StatConditionType.DefenseAtLeast)]
    public void 스탯이_기준_이상이면_참(StatConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, Stat(110, 120)));
        Assert.IsFalse(Check(type, threshold, Stat(80, 80)));
    }

    bool Check(StatConditionType type, int threshold, ChampionStatData target) => new StatThresholdChecker(type, threshold).Check(TestHelper.CreateStatus(target.Attack, target.Defense), TestHelper.CreateStatus());

    [Test]
    [TestCase(StatConditionType.AttackAtLeast)]
    [TestCase(StatConditionType.DefenseAtLeast)]
    public void 타겟이_더_크면_참(StatConditionType type)
    {
        var targetStat = Stat(10, 10);

        Assert.IsTrue(Check(type, TestHelper.CreateStatus(3, 8), targetStat));
        Assert.IsFalse(Check(type, TestHelper.CreateStatus(12, 10), targetStat));
    }

    [Test]
    [TestCase(StatConditionType.AttackBelow)]
    [TestCase(StatConditionType.DefenseBelow)]
    public void 타겟이_더_작으면_참(StatConditionType type)
    {
        var targetStat = Stat(10, 10);

        Assert.IsTrue(Check(type, TestHelper.CreateStatus(12, 11), targetStat));
        Assert.IsFalse(Check(type, TestHelper.CreateStatus(0, 4), targetStat));
    }

    bool Check(StatConditionType type, ChampionStatus user, ChampionStatData target) => new StatComparisonChecker(type).Check(TestHelper.CreateStatus(target.Attack, target.Defense), user);

    ChampionStatData Stat(int att = 0, int def = 0) => new ChampionStatData(att, def);
}
