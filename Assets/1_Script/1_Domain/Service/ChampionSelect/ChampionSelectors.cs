using PlasticPipe.Server;
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
    public int Pick(HashSet<int> candidateIds) => ranker.GetChampionRank(candidateIds).First();
}


public record ChampionValueLog(int Id, int Value);
public class ValuePickLog : IPickSelector // 데코레이트
{
    readonly IChampionEvaluator evaluator;
    readonly ValuePick valuePick;
    readonly ChampionCatalog catalog;
    public ValuePickLog(ValuePick valuePick, IChampionEvaluator evaluator, ChampionCatalog catalog)
    {
        this.catalog = catalog;
        this.evaluator = evaluator;
        this.valuePick = valuePick;
    }

    public int Pick(HashSet<int> candidateIds)
    {
        var logs = candidateIds
            .Select(id => new ChampionValueLog(id, evaluator.Evaluate(catalog.GetChampion(id))))
            .OrderByDescending(x => x.Value);
        UnityEngine.Debug.Log(string.Join("\n", logs));
        return valuePick.Pick(candidateIds);
    }
}