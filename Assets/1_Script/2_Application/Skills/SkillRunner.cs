using System.Collections.Generic;
using System.Linq;

public class SkillRunner
{
    readonly SkillActionFactory skillActionFactory;

    public SkillRunner(SkillActionFactory skillActionFactory)
    {
        this.skillActionFactory = skillActionFactory;
    }

    public void Run(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> selectTargets, Team team)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
        {
            var trigger = skillActionFactory.CreateTrigger(skillData.SkillUseTimingType, team);

            trigger.Register(() =>
            {
                foreach (var target in selectTargets.Where(x => skillData.Condition.Check(x, caster)))
                    skillActionFactory.CreateAction(skillData, caster, target, team).Do(target);
            });
        }
    }
}