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
            ExecuteSkill(action, condition, selectTargets, skillData.AmountData, caster, skillApplyTargetType);
        }
    }

    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> selectCards, SkillAmountData skillAmountData, ChampionStatus caster, SkillApplyTargetType skillApplyTargetType)
    {
        foreach (var selectCard in selectCards.Where(condition.Check))
        {
            int skillValue = SkillAmountCalculatorFactory.Create(skillAmountData, selectCard.Stat).Calculate(selectCard.Stat.Attack);
            if (SkillApplyTargetType.Select == skillApplyTargetType) action.Do(selectCard);
            else action.Do(caster);
        }
    }


    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}