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

public record ChampionValue(int Id, int Value);


public class ValuePick : IPickSelector
{
    readonly ChampionRanker ranker;
    public ValuePick(ChampionRanker ranker) => this.ranker = ranker;
    public int Pick(HashSet<int> candidateIds) => ranker.GetChampionRank(candidateIds).First().Id;
}

public class ChampionRanker
{
    readonly ChampionCatalog catalog;
    readonly ChampionValueCalculator calculator;

    public ChampionRanker(ChampionCatalog catalog, ChampionValueCalculator calculator)
    {
        this.catalog = catalog;
        this.calculator = calculator;
    }

    public IEnumerable<ChampionValue> GetChampionRank(IEnumerable<int> candidateIds)
    {
        return candidateIds
            .Select(id => new ChampionValue(id, calculator.Calculate(catalog.GetChampion(id))))
            .OrderByDescending(x => x.Value);
    }
}

public class ValueBan : IBanSelector
{
    readonly ChampionCatalog catalog;
    readonly ChampionValueCalculator scoreCalculator; // 분리된 계산기 사용

    public ValueBan(ChampionCatalog catalog, ChampionValueCalculator scoreCalculator)
    {
        this.catalog = catalog;
        this.scoreCalculator = scoreCalculator;
    }

    public int Pick(HashSet<int> candidateIds) => GetChampionRank(candidateIds).First().Id;

    public IEnumerable<ChampionValue> GetChampionRank(HashSet<int> candidateIds)
        => candidateIds
            .Select(id => new ChampionValue(id, scoreCalculator.Calculate(catalog.GetChampion(id))))
            .OrderByDescending(x => x.Value);

    public int Ban(HashSet<int> ids)
    {
        throw new System.NotImplementedException();
    }
}

public class ValuePickLog : IPickSelector
{
    readonly ChampionRanker championRanker;

    public ValuePickLog(ChampionRanker championRanker)
    {
        this.championRanker = championRanker;
    }

    public int Pick(HashSet<int> candidateIds)
    {
        var rank = championRanker.GetChampionRank(candidateIds);
        var logs = rank.Select(x => $"{x.Id} : {x.Value}");
        UnityEngine.Debug.Log(string.Join("\n", logs));
        return rank.First().Id;
    }
}