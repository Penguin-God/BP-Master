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
    readonly ChampionCatalog Catalog;
    readonly ChampionStatValueCalculator Evaluator;

    public ValuePick(ChampionCatalog catalog, ChampionStatValueCalculator valueEvaluator)
    {
        Catalog = catalog;
        Evaluator = valueEvaluator;
    }

    public int Pick(HashSet<int> ids) 
        => ids
            .OrderByDescending(x => GetValue(x))
            .First();

    int GetValue(int id) => Evaluator.CalculateStatValue(Catalog.GetChampion(id).Status.Stat);
}