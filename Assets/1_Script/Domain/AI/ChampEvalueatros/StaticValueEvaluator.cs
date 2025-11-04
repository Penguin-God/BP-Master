using System.Collections.Generic;
using System.Linq;

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
    
    public SkillEvaluator(SlotStorage<ChampionStatus> statuses) => this.statuses = statuses;

    public int Evaluate(SkillData skill, Team team)
    {
        if (skill.TargetRule.TargetSide == Side.All) return 0;

        int result = statuses.GetTeamCount(Team.Blue) * skill.Amount;
        if (skill.TargetRule.TargetSide == Side.Opponent) result *= -1;
        return result;
    }

    public int Evaluate(SkillExecutor executor, Team team)
    {
        return 0;
    }
}