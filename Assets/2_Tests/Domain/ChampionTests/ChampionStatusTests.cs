using NUnit.Framework;
using static TestHelper;

[TestFixture]
public class ChampionStatusTests
{
    [Test]
    public void 생성시_스탯이_정상적으로_저장된다()
    {
        var status = CreateStatus(att: 10, def: 5, speed: 3);

        Assert.AreEqual(10, status.Stat.Attack);
        Assert.AreEqual(5, status.Stat.Defense);
        Assert.AreEqual(3, status.Stat.Speed);
    }

    [Test]
    public void ChangeStat_호출시_스탯이_변경_후_이벤트()
    {
        var status = CreateStatus(1, 1, 1);
        ChampionStatData before = default;
        ChampionStatData after = default;
        status.OnStatChanged += (be, af) => (before, after) = (be, af);
        var newStat = CreateStat(5, 6, 7);

        status.ChangeStat(newStat);

        Assert.AreEqual(newStat, status.Stat);
        Assert.AreEqual(CreateStat(1, 1, 1), before);
        Assert.AreEqual(newStat, after);
    }
}
