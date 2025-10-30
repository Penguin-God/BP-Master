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
    {
        // 각 챔피언 ID에 대해 평가 점수 계산
        var scored = ids
            .Select(id =>
            {
                var champ = Catalog.GetChampionData(id);
                int value = Evaluator.Evaluate(id, champ.Stat);
                return new { Id = id, Value = value };
            });

        // 가장 높은 점수의 챔피언 선택
        return scored
            .OrderByDescending(x => x.Value)
            .First()
            .Id;
    }
}