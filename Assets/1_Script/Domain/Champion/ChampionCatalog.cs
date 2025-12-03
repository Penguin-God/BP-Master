using System.Collections.Generic;
using System.Linq;

public class Champion
{
    public readonly int Id;
    public readonly Skill Skill;
    public readonly ChampionStatus Status;

    public Champion(int id, Skill skill, ChampionStatus status)
    {
        Id = id;
        Skill = skill;
        Status = status;
    }
}

public class ChampionCatalog
{
    readonly IEnumerable<Champion> Champions;
    public ChampionCatalog(IEnumerable<Champion> champions) => Champions = champions;
    public Champion GetChampion(int id) => Champions.First(x => x.Id == id);
}
