using System;

public record ChampionProfile(int Id, string Name, ChampionStatData Stat, Skill Skill);

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
    readonly SkillTextBuilder _skillTextBuilder;
    
    public ChampionTextBuilder(Func<int, ChampionProfile> profileProvider, SkillTextBuilder skillTextBuilder)
    {
        _profileProvider = profileProvider;
        _skillTextBuilder = skillTextBuilder;
    }

    public ChampionTextModel Build(int id, ChampionStatData bonusStat)
    {
        var profile = _profileProvider(id);
        var statModel = ChampionStatModelPreserter.CreateStatModel(profile.Stat, bonusStat);
        var skillText = _skillTextBuilder.BuildSkillText(profile.Skill.SkillDatas);

        return new ChampionTextModel(profile.Name, statModel, skillText);
    }
}