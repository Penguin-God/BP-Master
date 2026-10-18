using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class ValueRankingTests
{
    [Test]
    public void 평가값이_높은_ID순으로_정렬하여_반환한다()
    {
        var c1 = CreateChampion(id: 100);
        var c2 = CreateChampion(id: 300);
        var c3 = CreateChampion(id: 500);
        ChampionCatalog catalog = CreateCaltalog(c1, c2, c3);
        var sut = new ChampionRanker(catalog, new ID_Evaluator());

        var result = sut.GetChampionRank(new HashSet<int> { 100, 300, 500 });

        CollectionAssert.AreEqual(new int[] { 500, 300, 100 }, result);
    }

    [Test]
    public void 카드를_평가한_데이터를_반환한다()
    {
        var c1 = CreateChampion(id: 100);
        var c2 = CreateChampion(id: 300);
        var c3 = CreateChampion(id: 500);
        ChampionCatalog catalog = CreateCaltalog(c1, c2, c3);
        var sut = new ChampionRanker(catalog, new ID_Evaluator());

        var result = sut.CreateCardValueList(new HashSet<int> { 100, 300, 500 });

        CollectionAssert.AreEqual(new HashSet<CardValue> { new(100, 100), new(300, 300), new(500, 500) }, result);
    }
}

public class ID_Evaluator : IChampionEvaluator
{
    public int Evaluate(Champion champion) => champion.Id;
}
