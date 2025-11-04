using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticValueEvaluator
{
    readonly Dictionary<int, int> masteryLevels = new();

    public StaticValueEvaluator(IEnumerable<ChampionMastery> masteries)
    {
        masteryLevels = masteries.ToDictionary(m => m.ChampionId, m => m.Level);
    }

    public int Evaluate(int championId, ChampionStatData stat)
    {
        masteryLevels.TryGetValue(championId, out int level);
        return stat.Attack + stat.Defense + level;
    }
}

public class SkillEvaluator
{
    SlotStorage<ChampionStatus> statuses;
    readonly TargetCounter Counter;
    readonly ChampionStatAverager StatAverager;

    public SkillEvaluator(SlotStorage<ChampionStatus> statuses) => this.statuses = statuses;

    public SkillEvaluator(SlotStorage<ChampionStatus> statuses, TargetCounter counter, ChampionStatAverager statAverager)
    {
        this.statuses = statuses;
        Counter = counter;
        StatAverager = statAverager;
    }

    public int Evaluate(SkillData skill, Team team)
    {
        if (skill.TargetRule.TargetSide == Side.All) return 0;

        int result = statuses.GetTeamCount(Team.Blue) * skill.Amount;
        if (skill.TargetRule.TargetSide == Side.Opponent) result *= -1;
        return result;
    }
}

public class ChampionStatAverager
{
    readonly IEnumerable<ChampionStatData> stats;

    public ChampionStatAverager(ChampionStatData[] stats) => this.stats = stats;

    public ChampionStatData GetStatAverage()
    {
        int totalAttack = stats.Sum(s => s.Attack);
        int totalDefense = stats.Sum(s => s.Defense);

        int attack = Mathf.RoundToInt(totalAttack / stats.Count());
        int defense = Mathf.RoundToInt(totalDefense / stats.Count());

        return new ChampionStatData(attack, defense, 0);
    }
}