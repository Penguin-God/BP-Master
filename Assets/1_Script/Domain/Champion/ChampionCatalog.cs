using System.Collections.Generic;

public class Champion
{
    public readonly Skill Skill;
    public readonly ChampionStatus Status;

    public Champion(Skill skill, ChampionStatus status)
    {
        Skill = skill;
        Status = status;
    }
}

public class ChampionCatalog
{
    readonly IReadOnlyDictionary<int, Champion> DataById;
    public ChampionCatalog(Dictionary<int, Champion> data) => DataById = data;
    public Champion GetChampion(int id) => DataById[id];
}
