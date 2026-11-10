using NUnit.Framework;
using static TestHelper;

public class SkillActionTests
{
    [Test]
    [TestCase(StatType.Attack)]
    [TestCase(StatType.Defense)]
    public void 챔피언_스탯_변경(StatType statType)
    {
        var target = CreateStatus(100, 100);
        var sut = new StatChanger(statType, () => 100);

        sut.Do(target);

        Assert.AreEqual(200, target.Stat.GetStatValue(statType));
    }
}
