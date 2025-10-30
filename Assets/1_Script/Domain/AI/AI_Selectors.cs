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

public class StaticValuePick : IPickSelector
{
    readonly ChampionCatalog Catalog;
    readonly StaticValueEvaluator Evaluator;

    public StaticValuePick(ChampionCatalog catalog, StaticValueEvaluator valueEvaluator)
    {
        Catalog = catalog;
        Evaluator = valueEvaluator;
    }

    public int Pick(HashSet<int> ids) 
        => ids
            .OrderByDescending(x => GetValue(x))
            .First();

    int GetValue(int id) => Evaluator.Evaluate(id, Catalog.GetChampionData(id).Stat);
}