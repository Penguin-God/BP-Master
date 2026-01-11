using System.Collections.Generic;
using System.Linq;

public interface IBanSelector
{
    public int Ban(HashSet<int> ids);
}

public interface IPickSelector
{
    public int Pick(HashSet<int> ids);
}

public class RandomBan : IBanSelector
{
    public int Ban(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
}

public class RandomPick : IPickSelector
{
    public int Pick(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
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