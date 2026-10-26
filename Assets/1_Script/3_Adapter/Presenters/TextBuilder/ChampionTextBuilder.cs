using System;
using System.Collections.Generic;

public record ChampionProfile(int Id, string Name, ChampionStatData Stat, IEnumerable<SkillViewModel> Skills);

public readonly struct ChampionTextModel
{
    public readonly string Name;
    public readonly ChampionStatModel StatModel;
    public readonly string SkillText;

    public ChampionTextModel(string name, ChampionStatModel stat, string skillText)
    {
        Name = name;
        StatModel = stat;
        SkillText = skillText;
    }
}

public class ChampionTextBuilder
{
    readonly Func<int, ChampionProfile> _profileProvider;
    readonly Func<IEnumerable<SkillViewModel>, string> _skillTextBuilder;

    public ChampionTextBuilder(Func<int, ChampionProfile> profileProvider, Func<IEnumerable<SkillViewModel>, string> skillTextBuilder)
    {
        _profileProvider = profileProvider;
        _skillTextBuilder = skillTextBuilder;
    }

    public ChampionTextModel Build(int id, ChampionStatData bonusStat)
    {
        var profile = _profileProvider(id);
        var statModel = ChampionStatModelPreserter.CreateStatModel(profile.Stat, bonusStat);
        var skillText = _skillTextBuilder(profile.Skills);

        return new ChampionTextModel(profile.Name, statModel, skillText);
    }
}
