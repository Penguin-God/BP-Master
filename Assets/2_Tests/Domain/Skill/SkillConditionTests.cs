using NUnit.Framework;

public class TraitConditionTests // StatThresholdChecker는 이상, 이하. StatComparisonChecker는 초과, 미만
{
    [Test]
    public void None은_무조건_참() => Assert.IsTrue(Check(StatConditionType.None, 0, default));

    [Test]
    public void NullChecker은_무조건_참() => Assert.IsTrue(new NullChecker().Check(default));

    [Test]
    [TestCase(StatConditionType.AttackBelow)]
    [TestCase(StatConditionType.DefenseBelow)]
    [TestCase(StatConditionType.SpeedBelow)]
    public void 스탯이_기준_이하면_참(StatConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, default));
        Assert.IsTrue(Check(type, threshold, Stat(100, 100, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(120, 120, 120)));
    }

    [Test]
    [TestCase(StatConditionType.AttackAtLeast)]
    [TestCase(StatConditionType.DefenseAtLeast)]
    [TestCase(StatConditionType.SpeedAtLeast)]
    public void 스탯이_기준_이상이면_참(StatConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, Stat(110, 120, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(80, 80, 80)));
    }

    bool Check(StatConditionType type, int threshold, ChampionStatData target) => new StatThresholdChecker(type, threshold).Check(target);

    [Test]
    [TestCase(StatConditionType.AttackAtLeast)]
    [TestCase(StatConditionType.DefenseAtLeast)]
    [TestCase(StatConditionType.SpeedAtLeast)]
    public void 타겟이_더_크면_참(StatConditionType type)
    {
        var targetStat = Stat(10, 10, 10);

        Assert.IsTrue(Check(type, Stat(3, 8, 0), targetStat));
        Assert.IsFalse(Check(type, Stat(12, 10, 22), targetStat));
    }

    [Test]
    [TestCase(StatConditionType.AttackBelow)]
    [TestCase(StatConditionType.DefenseBelow)]
    [TestCase(StatConditionType.SpeedBelow)]
    public void 타겟이_더_작으면_참(StatConditionType type)
    {
        var targetStat = Stat(10, 10, 10);

        Assert.IsTrue(Check(type, Stat(12, 11, 19), targetStat));
        Assert.IsFalse(Check(type, Stat(0, 4, 10), targetStat));
    }
    bool Check(StatConditionType type, ChampionStatData user, ChampionStatData target) => new StatComparisonChecker(type, user).Check(target);

    ChampionStatData Stat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
