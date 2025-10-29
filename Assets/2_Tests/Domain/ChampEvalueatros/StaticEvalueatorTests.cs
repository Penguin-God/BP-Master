using NUnit.Framework;
using System.Collections.Generic;

public class StaticEvalueatorTests
{
    [Test]
    [TestCase(1, 75)]
    [TestCase(2, 60)]
    public void 정적_가치는_공방과_숙련도의_합(int champId, int excpected)
    {
        IEnumerable<ChampionMastery> masteries = new ChampionMastery[] { new ChampionMastery(1, 15) };
        var sut = new StaticValueEvaluator(masteries);
        ChampionStatData stat = TestHelper.CreateStat(att: 20, def: 40);

        int result = sut.Evaluate(champId, stat);

        Assert.AreEqual(excpected, result);
    }
}
