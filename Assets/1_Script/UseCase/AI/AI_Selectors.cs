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
    readonly ChampionCatalog catalog;
    readonly ChampionValueCalculator scoreCalculator; // 분리된 계산기 사용

    public ValuePick(ChampionCatalog catalog, ChampionValueCalculator scoreCalculator)
    {
        this.catalog = catalog;
        this.scoreCalculator = scoreCalculator;
    }

    public int Pick(HashSet<int> candidateIds)
        => candidateIds
            .Select(id => catalog.GetChampion(id))
            .OrderByDescending(scoreCalculator.Calculate) // 메서드 참조 전달
            .First()
            .Id;
}

public class ValuePickLog : IPickSelector
{
    readonly ChampionCatalog catalog;

    public ValuePickLog(ChampionCatalog catalog, ChampionStatValueCalculator statCalculator, SkillApplyDeltaCalculator deltaCalculator, MasteryCollection masteryCollection, Team team)
    {
        this.catalog = catalog;
        this.statCalculator = statCalculator;
        this.deltaCalculator = deltaCalculator;
        this.masteryCollection = masteryCollection;
        this.myTeam = team;
    }

    public int Pick(HashSet<int> candidateIds)
    {
        var result = candidateIds
            .Select(id => catalog.GetChampion(id))
            .OrderByDescending(CalculateScore) // 점수 내림차순 정렬
            .First()
            .Id;

        UnityEngine.Debug.Log(string.Join("\n", values.OrderByDescending(x => x.Value).Select(x => $"{x.Key} : {x.Value}")));
        values.Clear();
        return result;
    }

    Dictionary<int, int> values = new();

    readonly ChampionStatValueCalculator statCalculator;
    readonly SkillApplyDeltaCalculator deltaCalculator;
    readonly MasteryCollection masteryCollection;
    readonly Team myTeam;
    int CalculateScore(Champion champion)
    {
        int statScore = statCalculator.CalculateStatValue(champion.Status.Stat);
        int masteryScore = masteryCollection.GetMasteryLevel(champion.Id) * 2;

        var statChangeInfo = deltaCalculator.CalculateApplySkillStat(champion);
        int skillScore = statCalculator.CalcualteTeamStatValue(statChangeInfo, myTeam);
        values.Add(champion.Id, statScore + masteryScore + skillScore);
        return statScore + masteryScore + skillScore;
    }
}