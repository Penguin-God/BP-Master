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

public class ValuePick : IPickSelector // 로그용 인터페이스 따로 만들기
{
    readonly ChampionCatalog catalog;

    public ValuePick(ChampionCatalog catalog, ChampionStatValueCalculator statCalculator, SkillApplyDeltaCalculator deltaCalculator, MasteryCollection masteryCollection, Team team)
    {
        this.catalog = catalog;
        this.statCalculator = statCalculator;
        this.deltaCalculator = deltaCalculator;
        this.masteryCollection = masteryCollection;
        this.myTeam = team;
    }

    public int Pick(HashSet<int> candidateIds)
    {
        // 후보군 중 점수(Score)가 가장 높은 챔피언의 ID 반환
        //return candidateIds
        //    .Select(id => catalog.GetChampion(id))
        //    .OrderByDescending(CalculateScore) // 점수 내림차순 정렬
        //    .First()
        //    .Id;

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