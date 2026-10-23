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

            trigger.Register(triggeredChampion =>
            {
                var candidateTargets = skillData.SkillApplyType == SkillApplyType.Triggered && triggeredChampion != null ? new[] { triggeredChampion } : selectTargets;

                foreach (var select in candidateTargets.Where(x => skillData.Condition.Check(x, caster)))
                {
                    var actualTarget = SkillFuncProvider.ResolveTarget(skillData, select, caster);
                    skillActionFactory.CreateAction(skillData, caster, select, team).Do(actualTarget);
                }
            });
        }
    }
}