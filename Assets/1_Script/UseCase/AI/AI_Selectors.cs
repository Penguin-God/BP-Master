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
    readonly ChampionCatalog catalog;
    readonly ChampionValueCalculator scoreCalculator; // 분리된 계산기 사용

    public ValuePick(ChampionCatalog catalog, ChampionValueCalculator scoreCalculator)
    {
        this.catalog = catalog;
        this.scoreCalculator = scoreCalculator;
    }

    public int Pick(HashSet<int> candidateIds) => GetChampionRank(candidateIds).First().Id;

    public IEnumerable<ChampionValue> GetChampionRank(HashSet<int> candidateIds)
        => candidateIds
            .Select(id => new ChampionValue(id, scoreCalculator.Calculate(catalog.GetChampion(id))))
            .OrderByDescending(x => x.Value);
}

public class ValuePickLog : IPickSelector
{
    readonly ValuePick valuePick;

    public ValuePickLog(ValuePick valuePick)
    {
        this.valuePick = valuePick;
    }

    public int Pick(HashSet<int> candidateIds)
    {
        var rank = valuePick.GetChampionRank(candidateIds);
        var logs = rank.Select(x => $"{x.Id} : {x.Value}");
        UnityEngine.Debug.Log(string.Join("\n", logs));
        return rank.First().Id;
    }
}