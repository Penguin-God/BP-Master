using System;
using System.Collections.Generic;
using System.Linq;

public class RandomSelector : IChampionSelector
{
    public int Select(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
}

public class ValueSelector : IChampionSelector
{
    readonly ChampionCatalog catalog;
    readonly Func<Champion, int> evaluator;

    public ValueSelector(ChampionCatalog catalog, Func<Champion, int> evaluator)
    {
        this.catalog = catalog;
        this.evaluator = evaluator;
    }
    public int Select(HashSet<int> ids) => CreateCardValueList(ids).First().Id;
    public IEnumerable<CardValue> CreateCardValueList(IEnumerable<int> candidateIds)  => candidateIds.Select(id => new CardValue(id, evaluator(catalog.GetChampion(id)))).OrderByDescending(x => x.Value);
}

public class ValueSelectorLog : IChampionSelector
{
    readonly ValueSelector selector;
    public ValueSelectorLog(ValueSelector selector) => this.selector = selector;

    public int Select(HashSet<int> ids)
    {
        var ranked = selector.CreateCardValueList(ids);
        var logs = ranked.Select(x => $"Id : {x.Id}, Value : {x.Value}");
        UnityEngine.Debug.Log(string.Join("\n", logs));

        return selector.Select(ids);
    }
}

public record CardValue(int Id, int Value);