using System.Collections.Generic;
using System.Linq;

public interface IChampionEvaluator
{
    public int Evaluate(Champion champion);
}

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
}