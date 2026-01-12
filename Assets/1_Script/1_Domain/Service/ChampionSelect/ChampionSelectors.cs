using System.Collections.Generic;
using System.Linq;

public interface IChampionSelector
{
    public int Select(HashSet<int> ids);
}

public class RandomSelector : IChampionSelector
{
    public int Select(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
}

public class ValueSelector : IChampionSelector
{
    readonly ChampionRanker ranker;
    public ValueSelector(ChampionRanker ranker) => this.ranker = ranker;
    public int Select(HashSet<int> ids) => ranker.GetChampionRank(ids).First();
}

public class ValueSelectorLog : IChampionSelector
{
    readonly ValueSelector selector;
    readonly ValueLogger logger;

    public ValueSelectorLog(ValueSelector selector, ValueLogger logger)
    {
        this.selector = selector;
        this.logger = logger;
    }

    public int Select(HashSet<int> ids)
    {
        logger.LogValue(ids);
        return selector.Select(ids);
    }
}

public record ChampionValueLog(int Id, int Value);

public class ValueLogger
{
    readonly ChampionCatalog catalog;
    readonly IChampionEvaluator evaluator;

    public ValueLogger(ChampionCatalog catalog, IChampionEvaluator evaluator)
    {
        this.catalog = catalog;
        this.evaluator = evaluator;
    }

    public void LogValue(HashSet<int> ids)
    {
        var logs = ids
            .Select(id => new ChampionValueLog(id, evaluator.Evaluate(catalog.GetChampion(id))))
            .OrderByDescending(x => x.Value)
            .Select(x => $"Id : {x.Id}, Value : {x.Value}");
        UnityEngine.Debug.Log(string.Join("\n", logs));
    }
}