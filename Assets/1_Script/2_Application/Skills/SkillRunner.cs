using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

public enum SkillApplyTargetType { Select, OneSelf, }

public class SkillRunner
{
    readonly SkillActionFactory skillActionFactory;
    public SkillRunner(SkillActionFactory skillActionFactory)
    {
        this.skillActionFactory = skillActionFactory;
    }

    public void Run(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> targets, Team team)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
        {
            ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, skillData.AmountData, caster, team);
            IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, caster.Stat);
            ExecuteSkill(action, condition, targets);
        }
    }

    public void Run(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> selectTargets, Team team, SkillApplyTargetType skillApplyTargetType)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
        {
            ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, skillData.AmountData, caster, team);
            IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, caster.Stat);
            ExecuteSkill(action, condition, selectTargets, skillData.AmountData);
        }
    }

    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets, SkillAmountData skillAmountData)
    {
        foreach (var target in targets.Where(condition.Check))
        {
            int skillValue = SkillAmountCalculatorFactory.Create(skillAmountData, target.Stat).Calculate(target.Stat.Attack);
            action.Do(target);
        }
    }


    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}