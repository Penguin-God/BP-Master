using NUnit.Framework;

public class TraitConditionTests
{
    [Test]
    public void None은_무조건_참() => Assert.IsTrue(Check(TraitConditionType.None, default, 0));

    [Test]
    [TestCase(TraitConditionType.AttackBelow)]
    [TestCase(TraitConditionType.DefenseBelow)]
    [TestCase(TraitConditionType.SpeedBelow)]
    public void 스탯이_기준_이하면_참2(TraitConditionType type)
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
    public void 스탯이_기준_이상이면_참2(TraitConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, threshold, Stat(110, 120, 100)));
        Assert.IsFalse(Check(type, threshold, Stat(80, 80, 80)));
    }

    bool Check(TraitConditionType type, int threshold, ChampionStatData target) => new StatThresholdChecker(type, threshold).Check(target);

    [Test]
    [TestCase(TraitConditionType.AttackBelow)]
    [TestCase(TraitConditionType.DefenseBelow)]
    [TestCase(TraitConditionType.SpeedBelow)]
    public void 스탯이_기준_이하면_참(TraitConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, default, threshold));
        Assert.IsTrue(Check(type, Stat(100, 100, 100), threshold));
        Assert.IsFalse(Check(type, Stat(120, 120, 120), threshold));
    }

    [Test]
    [TestCase(TraitConditionType.AttackAtLeast)]
    [TestCase(TraitConditionType.DefenseAtLeast)]
    [TestCase(TraitConditionType.SpeedAtLeast)]
    public void 스탯이_기준_이상이면_참(TraitConditionType type)
    {
        int threshold = 100;

        Assert.IsTrue(Check(type, Stat(110, 120, 100), threshold));
        Assert.IsFalse(Check(type, Stat(80, 80, 80), threshold));
    }

    [Test]
    [TestCase(TraitConditionType.AttackAtLeast)]
    [TestCase(TraitConditionType.DefenseAtLeast)]
    [TestCase(TraitConditionType.SpeedAtLeast)]
    public void 타겟과_비교_후_높으면_true(TraitConditionType type)
    {
        var statData = Stat(10, 10, 10);
        var data = CreateCondition(type, 0, true);

        Assert.IsTrue(Check(data, statData, Stat(6, 6, 10)));
        Assert.IsFalse(Check(data, statData, Stat(12, 12, 12)));
    }

    bool Check(TraitConditionType type, ChampionStatData stat, int threshold) => new TraitConditionChecker().CheckCondition(new TraitConditionData(type, threshold, false), default, targetStat: stat);
    bool Check(TraitConditionData data, ChampionStatData user, ChampionStatData target) => new TraitConditionChecker().CheckCondition(data, user, target);
    TraitConditionData CreateCondition(TraitConditionType type, int threshold, bool isCompare) => new TraitConditionData(type, threshold, isCompare);

    ChampionStatData Stat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
