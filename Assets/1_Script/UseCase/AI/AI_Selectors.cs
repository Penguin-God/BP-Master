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
    readonly ChampionStatValueCalculator statCalculator;
    readonly SkillApplyDeltaCalculator deltaCalculator;
    readonly MasteryCollection masteryCollection;
    readonly Team myTeam;

    // 생성자: 테스트 코드 시그니처에 맞춤 (team은 선택적 파라미터로 처리하여 기존 테스트 호환)
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
        return candidateIds
            .Select(id => catalog.GetChampion(id))
            .OrderByDescending(CalculateScore) // 점수 내림차순 정렬
            .First()
            .Id;
    }

    int CalculateScore(Champion champion)
    {
        int statScore = statCalculator.CalculateStatValue(champion.Status.Stat);
        int masteryScore = masteryCollection.GetMasteryLevel(champion.Id) * 2;

        var statChangeInfo = deltaCalculator.CalculateApplySkillStat(champion);
        int skillScore = statCalculator.CalcualteTeamStatValue(statChangeInfo, myTeam);
        return statScore + masteryScore + skillScore;
    }
}