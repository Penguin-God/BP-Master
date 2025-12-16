using System.Collections.Generic;
using System.Linq;

public class StatTeamPredictor
{
    readonly ChampionStatAverager StatAverager;
    readonly int TeamSize;

    public StatTeamPredictor(ChampionStatAverager statAverager, int teamSize)
    {
        StatAverager = statAverager;
        TeamSize = teamSize;
    }

    public SlotStorage<ChampionStatus> BuildPerdictTeam(SlotStorage<ChampionStatus> statuses)
    {
        ChampionStatData average = StatAverager.GetStatAverage();

        FillTeamWithAverage(statuses, Team.Blue, average);
        FillTeamWithAverage(statuses, Team.Red, average);

        return statuses;
    }

    void FillTeamWithAverage(SlotStorage<ChampionStatus> statuses, Team team, ChampionStatData average)
    {
        int currentCount = statuses.GetTeamCount(team);
        int needCount = TeamSize - currentCount;
        if (needCount <= 0) return;

        // 평균 스탯으로 빈 슬롯 채우기
        Enumerable.Repeat(0, needCount)
                  .ToList()
                  .ForEach(_ => statuses.AddSlot(team, new ChampionStatus(average, TraitType.None)));
    }
}


public class ChampionStatAverager
{
    readonly IEnumerable<ChampionStatData> stats;
    public ChampionStatAverager(IEnumerable<ChampionStatData> stats) => this.stats = stats;

    public ChampionStatData GetStatAverage()
    {
        int totalAttack = stats.Sum(s => s.Attack);
        int totalDefense = stats.Sum(s => s.Defense);

        int attack = totalAttack / stats.Count();
        int defense = totalDefense / stats.Count();

        return new ChampionStatData(attack, defense, 0);
    }
}