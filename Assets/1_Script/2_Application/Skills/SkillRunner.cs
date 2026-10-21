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
            IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, caster.Stat);
            ExecuteSkill(condition, selectTargets, skillData, caster, skillApplyTargetType);
        }
    }

    void ExecuteSkill(IChampionCondition condition, IEnumerable<ChampionStatus> selectCards, SkillData skillData, ChampionStatus caster, SkillApplyTargetType skillApplyTargetType)
    {
        foreach (var selectCard in selectCards.Where(condition.Check))
        {
            int value = SkillAmountCalculatorFactory.Create(skillData.AmountData, selectCard.Stat).Calculate(selectCard.GetStatAmount(skillData.AmountData.StatType));
            if (SkillApplyTargetType.Select == skillApplyTargetType) selectCard.ChangeStat(value, skillData.AmountData.StatType);
            else caster.ChangeStat(value, skillData.AmountData.StatType);
        }
    }


    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}