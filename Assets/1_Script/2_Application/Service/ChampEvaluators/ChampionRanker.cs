using System.Collections.Generic;
using System.Linq;

public class ChampionRanker
{
    readonly ChampionCatalog catalog;
    readonly IChampionEvaluator evaluator;

    public ChampionRanker(ChampionCatalog catalog, IChampionEvaluator evaluator)
    {
        this.catalog = catalog;
        this.evaluator = evaluator;
    }

    public IEnumerable<int> GetChampionRank(IEnumerable<int> candidateIds) => candidateIds.OrderByDescending(x => evaluator.Evaluate(catalog.GetChampion(x)));

    public IEnumerable<CardValue> CreateCardValueList(IEnumerable<int> candidateIds) => candidateIds.Select(id => new CardValue(id, evaluator.Evaluate(catalog.GetChampion(id))));
}