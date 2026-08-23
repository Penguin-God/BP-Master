using System;

public record ChampionProfile(int Id, string Name, ChampionStatData Stat, Skill Skill);

public readonly struct ChampionTextModel
{
    public readonly string Name;
    public readonly ChampionStatData Stat;
    public readonly ChampionStatModel StatModel;
    public readonly string SkillText;

    public ChampionTextModel(string name, ChampionStatData stat)
    {
        Name = name;
        Stat = stat;
        StatModel = default;
        SkillText = "";
    }

    public ChampionTextModel(string name, ChampionStatModel stat, string skillText)
    {
        Name = name;
        StatModel = stat;
        SkillText = skillText;
        Stat = default;
    }
}

public class ChampionTextBuilder
{
    readonly Func<int, ChampionProfile> _profileProvider;
    readonly SkillTextBuilder _skillTextBuilder;
    readonly ChampionStatusTextBuilder _statTextBuilder;

    public ChampionTextBuilder(Func<int, ChampionProfile> profileProvider, SkillTextBuilder skillTextBuilder, ChampionStatusTextBuilder statTextBuilder)
    {
        _profileProvider = profileProvider;
        _skillTextBuilder = skillTextBuilder;
        _statTextBuilder = statTextBuilder;
    }

    public ChampionTextModel Build(int id)
    {
        var profile = _profileProvider(id); // 람다 실행
        var statModel = _statTextBuilder.CreateStatViewModel(profile.Stat);
        var skillText = _skillTextBuilder.BuildSkillText(profile.Skill.SkillDatas);

        return new ChampionTextModel(profile.Name, statModel, skillText);
    }
}