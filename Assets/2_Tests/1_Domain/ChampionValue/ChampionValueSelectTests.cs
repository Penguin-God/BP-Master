using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class ChampionValueSelectTests
{
    [Test]
    public void 가장_가치가_높은_챔피언_픽()
    {
        ChampionCatalog catalog = CreateCaltalog(CreateChampion(id: 1), CreateChampion(id: 2), CreateChampion(id: 3));
        ValuePick sut = new ValuePick(new ChampionRanker(catalog, new ID_Evaluator()));

        int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        Assert.AreEqual(3, result);
    }
}
