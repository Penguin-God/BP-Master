using NUnit.Framework;

public class TraitActionTests
{
    [Test]
    [TestCase(10, 22)]
    [TestCase(-10, 2)]
    public void 공_변경(int amount, int expected)
    {
        AttackChanger sut = new(amount);
        var data = CreateStat(12);

        var result = sut.Do(data);

        Assert.AreEqual(expected, result.Attack);
    }

    [Test]
    [TestCase(5, 15)]
    [TestCase(-3, 7)]
    public void 방_변경(int amount, int expected)
    {
        DefenseChanger sut = new(amount);
        var data = CreateStat(def: 10);

        var result = sut.Do(data);

        Assert.AreEqual(expected, result.Defense);
    }

    [Test]
    [TestCase(2, 12)]
    [TestCase(-5, 5)]
    public void 속_변경(int amount, int expected)
    {
        SpeedChanger sut = new(amount);
        var data = CreateStat(speed: 10);

        var result = sut.Do(data);

        Assert.AreEqual(expected, result.Speed);
    }

    ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
