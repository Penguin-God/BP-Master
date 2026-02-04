using System.Collections.Generic;

public class SkillRunner
{
    private readonly SkillExecutorFactory _factory;

    public SkillRunner(SkillExecutorFactory factory)
    {
        _factory = factory;
    }

    public void Run(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> targets)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
            _factory.CreateExecutor(skillData, caster).ExecuteSkill(targets);
    }
}