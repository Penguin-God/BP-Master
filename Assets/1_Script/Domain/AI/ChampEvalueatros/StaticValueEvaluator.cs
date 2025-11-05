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
    readonly StatTeamPredictor statTeamPredictor;
    public SkillEvaluator(SlotStorage<ChampionStatus> statuses) => this.statuses = statuses;

    public SkillEvaluator(SlotStorage<ChampionStatus> statuses, StatTeamPredictor statTeamPredictor)
    {
        this.statuses = statuses;
        this.statTeamPredictor = statTeamPredictor;
    }

    public int Evaluate(SkillExecutor executor, Team team, TraitTargetRule rule)
    {
        
        return 0;
    }
}