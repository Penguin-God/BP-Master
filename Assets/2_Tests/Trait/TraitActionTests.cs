using NUnit.Framework;

public class TraitActionTests
{
    [Test]
    public void 챔피언_공_변경()
    {
        var target = TestHelper.CreateStatChamp(11, 0, 0);
        AttackChanger sut = new(10);

        sut.Do(target);

        Assert.AreEqual(21, target.StatData.Attack);
    }

    [Test]
    public void 챔피언_방_변경()
    {
        var target = TestHelper.CreateStatChamp(0, 10, 0);
        var sut = new DefenseChanger(5);

        sut.Do(target);

        Assert.AreEqual(15, target.StatData.Defense);
    }

    [Test]
    public void 챔피언_속_변경()
    {
        var target = TestHelper.CreateStatChamp(0, 0, 7);
        var sut = new SpeedChanger(3);

        sut.Do(target);

        Assert.AreEqual(10, target.StatData.Speed);
    }

    ChampionStatData CreateStat(int att = 0, int def = 0, int speed = 0) => new ChampionStatData(att, def, speed);
}
