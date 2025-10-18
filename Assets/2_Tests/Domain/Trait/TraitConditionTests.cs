using NUnit.Framework;

public class TraitConditionTests // StatThresholdChecker는 이상, 이하. StatComparisonChecker는 초과, 미만
{
    [Test]
    public void None은_무조건_참() => Assert.IsTrue(Check(TraitConditionType.None, 0, default));

    [Test]
    public void NullChecker은_무조건_참() => Assert.IsTrue(new NullChecker().Check(default));

    [Test]
    [TestCase(TraitConditionType.AttackBelow)]
    [TestCase(TraitConditionType.DefenseBelow)]
    [TestCase(TraitConditionType.SpeedBelow)]
    public void 스탯이_기준_이하면_참(TraitConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, default));
        Assert.IsTrue(Check(type, threshold, Stat(100, 100, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(120, 120, 120)));
    }

    [Test]
    [TestCase(TraitConditionType.AttackAtLeast)]
    [TestCase(TraitConditionType.DefenseAtLeast)]
    [TestCase(TraitConditionType.SpeedAtLeast)]
    public void 스탯이_기준_이상이면_참(TraitConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, Stat(110, 120, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(80, 80, 80)));
    }

    bool Check(TraitConditionType type, int threshold, ChampionStatData target) => new StatThresholdChecker(type, threshold).Check(target);

    [Test]
    [TestCase(TraitConditionType.AttackAtLeast)]
    [TestCase(TraitConditionType.DefenseAtLeast)]
    [TestCase(TraitConditionType.SpeedAtLeast)]
    public void 타겟과_비교_후_초과면_참(TraitConditionType type)
    {
        var targetStat = Stat(10, 10, 10);

        Assert.IsTrue(Check(type, Stat(12, 15, 11), targetStat));
        Assert.IsFalse(Check(type, Stat(8, 10, 2), targetStat));
    }

    [Test]
    [TestCase(TraitConditionType.AttackBelow)]
    [TestCase(TraitConditionType.DefenseBelow)]
    [TestCase(TraitConditionType.SpeedBelow)]
    public void 타겟과_비교_후_미만이면_참(TraitConditionType type)
    {
        var targetStat = Stat(10, 10, 10);

        Assert.IsTrue(Check(type, Stat(6, 8, 9), targetStat));
        Assert.IsFalse(Check(type, Stat(12, 10, 12), targetStat));
    }
    bool Check(TraitConditionType type, ChampionStatData user, ChampionStatData target) => new StatComparisonChecker(type, user).Check(target);

    ChampionStatData Stat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
