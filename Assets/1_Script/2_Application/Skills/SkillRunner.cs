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
                // Triggered 타입이면 이벤트로 넘어온 챔피언 1개만 대상, 아니면 기존 선택 대상들 사용
                var actualTargets = skillData.SkillApplyType == SkillApplyType.Triggered && triggeredChampion != null ? new[] { triggeredChampion } : selectTargets;

                foreach (var target in actualTargets.Where(x => skillData.Condition.Check(x, caster)))
                    skillActionFactory.CreateAction(skillData, caster, target, team).Do(target);
            });
        }
    }
}