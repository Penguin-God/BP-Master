using System.Collections.Generic;

public readonly struct ChampionData
{
    public readonly ChampionStatData Stat;
    public readonly TraitType TraitType;
    public readonly SkillData SkillData;

    public ChampionData(ChampionStatData stat, TraitType traitType, SkillData skillData)
    {
        Stat = stat;
        TraitType = traitType;
        SkillData = skillData;
    }
}

public class ChampionCatalog
{
    readonly IReadOnlyDictionary<int, ChampionData> DataById;
    public ChampionCatalog(Dictionary<int, ChampionData> data) => DataById = data;
    public ChampionData GetChampionData(int id) => DataById[id];
}
