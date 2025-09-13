public interface ITraitCondition
{
    bool Condition(ChampionStatData stat);
}

public class DefenseBelowCondition : ITraitCondition
{
    readonly int defenseThreshold;
    public DefenseBelowCondition(int defenseThreshold) => this.defenseThreshold = defenseThreshold;
    public bool Condition(ChampionStatData stat) => stat.Defense < defenseThreshold;
}