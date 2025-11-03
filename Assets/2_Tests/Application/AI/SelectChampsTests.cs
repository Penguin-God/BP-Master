using NUnit.Framework;
using System.Collections.Generic;

public class SelectChampsTests
{
    [Test]
    public void 가장_정적_가치가_높은_챔피언_픽()
    {
        ChampionCatalog catalog = new ChampionCatalog(new Dictionary<int, ChampionData>()
        {
            {1, Champ(10, 20) },
            {2, Champ(10, 50) },
            {3, Champ(100, 50) },
        });
        StaticValueEvaluator evaluator = new StaticValueEvaluator(new ChampionMastery[0]);
        StaticValuePick sut = new StaticValuePick(catalog, evaluator);

        int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        Assert.AreEqual(3, result);
    }

    ChampionData Champ(int att, int def) => new ChampionData(TestHelper.CreateStat(att, def), TraitType.None, default);
}
