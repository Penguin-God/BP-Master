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
    readonly int TeamSize;
    public SkillEvaluator(SlotStorage<ChampionStatus> statuses) => this.statuses = statuses;

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
    public ChampionStatData GetStatAverage(ChampionStatData[] stats)
    {
        int totalAttack = stats.Sum(s => s.Attack);
        int totalDefense = stats.Sum(s => s.Defense);

        int attack = Mathf.RoundToInt(totalAttack / stats.Length);
        int defense = Mathf.RoundToInt(totalDefense / stats.Length);

        return new ChampionStatData(attack, defense, 0);
    }
}