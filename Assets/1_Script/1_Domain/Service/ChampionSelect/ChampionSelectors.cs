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

public interface IBanSelector
{
    public int Ban(HashSet<int> ids);
}

public interface IPickSelector
{
    public int Pick(HashSet<int> ids);
}

public class ValuePick : IPickSelector
{
    readonly ChampionRanker ranker;
    public ValuePick(ChampionRanker ranker) => this.ranker = ranker;
    public int Pick(HashSet<int> ids) => ranker.GetChampionRank(ids).First();
}

public class ValueBan : IBanSelector
{
    readonly ChampionRanker ranker;
    public ValueBan(ChampionRanker ranker) => this.ranker = ranker;
    public int Ban(HashSet<int> ids) => ranker.GetChampionRank(ids).First();
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

public class ValuePickLog : IPickSelector
{
    readonly ValuePick valuePick;
    readonly ValueLogger valueLogger;

    public ValuePickLog(ValuePick valuePick, ValueLogger valueLogger)
    {
        this.valuePick = valuePick;
        this.valueLogger = valueLogger;
    }

    public int Pick(HashSet<int> ids)
    {
        valueLogger.LogValue(ids);
        return valuePick.Pick(ids);
    }
}


public class ValueBanLog : IBanSelector
{
    readonly ValueBan valueBan;
    readonly ValueLogger valueLogger;

    public ValueBanLog(ValueBan valuePick, ValueLogger valueLogger)
    {
        this.valueBan = valuePick;
        this.valueLogger = valueLogger;
    }

    public int Ban(HashSet<int> ids)
    {
        valueLogger.LogValue(ids);
        return valueBan.Ban(ids);
    }
}