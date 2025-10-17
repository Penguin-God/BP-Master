using NUnit.Framework;

public class TraitConditionTests
{
    [Test]
    public void None은_무조건_참()
    {
        Assert.IsTrue(Check(TraitConditionType.None, default, 0));
    }

    [Test]
    public void 방어력이_기준점_이하면_참()
    {
        int threshold = 100;

        Assert.IsTrue(Check(TraitConditionType.DefenseBelow, default, threshold));
        Assert.IsTrue(Check(TraitConditionType.DefenseBelow, Stat(def: 100), threshold));
        Assert.IsFalse(Check(TraitConditionType.DefenseBelow, Stat(def: 120), threshold));
    }

    [Test]
    public void 방어력이_기준점_이상이면_참()
    {
        int threshold = 100;

        Assert.IsFalse(Check(TraitConditionType.DefenseAtLeast, Stat(def: 50), threshold));
        Assert.IsTrue(Check(TraitConditionType.DefenseAtLeast, Stat(def: 100), threshold));
        Assert.IsTrue(Check(TraitConditionType.DefenseAtLeast, Stat(def: 120), threshold));
    }

    [Test]
    public void 공격력이_기준점_이하면_참()
    {
        int threshold = 50;

        Assert.IsTrue(Check(TraitConditionType.AttackBelow, default, threshold));
        Assert.IsTrue(Check(TraitConditionType.AttackBelow, Stat(att: 50), threshold));
        Assert.IsFalse(Check(TraitConditionType.AttackBelow, Stat(att: 70), threshold));
    }

    [Test]
    public void 공격력이_기준점_이상이면_참()
    {
        int threshold = 50;

        Assert.IsFalse(Check(TraitConditionType.AttackAtLeast, Stat(att: 30), threshold));
        Assert.IsTrue(Check(TraitConditionType.AttackAtLeast, Stat(att: 50), threshold));
        Assert.IsTrue(Check(TraitConditionType.AttackAtLeast, Stat(att: 70), threshold));
    }

    [Test]
    public void 속도가_기준점_이하면_참()
    {
        int threshold = 10;

        Assert.IsTrue(Check(TraitConditionType.SpeedBelow, default, threshold));
        Assert.IsTrue(Check(TraitConditionType.SpeedBelow, Stat(speed: 10), threshold));
        Assert.IsFalse(Check(TraitConditionType.SpeedBelow, Stat(speed: 20), threshold));
    }

    [Test]
    public void 속도가_기준점_이상이면_참()
    {
        int threshold = 10;

        Assert.IsFalse(Check(TraitConditionType.SpeedAtLeast, Stat(speed: 5), threshold));
        Assert.IsTrue(Check(TraitConditionType.SpeedAtLeast, Stat(speed: 10), threshold));
        Assert.IsTrue(Check(TraitConditionType.SpeedAtLeast, Stat(speed: 20), threshold));
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

    bool Check(TraitConditionType type, ChampionStatData stat, int threshold) => new TraitConditionChecker().CheckCondition(new TraitConditionData(type, threshold), stat);
    bool Check(TraitConditionData data, ChampionStatData user, ChampionStatData target) => new TraitConditionChecker().CheckCondition(data, user, target);
    TraitConditionData CreateCondition(TraitConditionType type, int threshold, bool isCompare) => new TraitConditionData(type, threshold, isCompare);

    ChampionStatData Stat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
