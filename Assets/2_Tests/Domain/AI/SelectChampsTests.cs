using NUnit.Framework;
using System.Collections.Generic;

public class SelectChampsTests
{
    [Test]
    public void 가장_정적_가치가_높은_챔피언_픽()
    {
        ChampionCatalog catalog = new ChampionCatalog(new Champion[] { Champ(1, 10, 20), Champ(2, 10, 50), Champ(3, 100, 50) });
        StaticValueEvaluator evaluator = new StaticValueEvaluator(new ChampionMastery[0]);
        StaticValuePick sut = new StaticValuePick(catalog, evaluator);

        int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        Assert.AreEqual(3, result);
    }

    Champion Champ(int id, int att, int def) => new Champion(id, new Skill(null), TestHelper.CreateStatus(att, def));
}
