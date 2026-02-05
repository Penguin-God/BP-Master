using System.Collections.Generic;

public class SkillRunner
{
    readonly ISkillActionFactory skillActionFactory;
    readonly SkillCondtionFactory skillCondtionFactory;
    public SkillRunner(ISkillActionFactory skillActionFactory, SkillCondtionFactory skillCondtionFactory)
    {
        this.skillActionFactory = skillActionFactory;
        this.skillCondtionFactory = skillCondtionFactory;
    }

    public void Run(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> targets, Team team)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
        {
            ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, skillData.AmountData, caster, team);
            IChampionCondition condition = skillCondtionFactory.CreateCondition(skillData.ConditionData, caster.Stat);
            new SkillExecutor(action, condition).ExecuteSkill(targets);
        }
    }
}