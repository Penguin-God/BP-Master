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
            ExecuteSkill(condition, selectTargets, skillData, caster, skillApplyTargetType, team);
        }
    }

    void ExecuteSkill(IChampionCondition condition, IEnumerable<ChampionStatus> selectCards, SkillData skillData, ChampionStatus caster, SkillApplyTargetType skillApplyTargetType, Team team)
    {
        foreach (var selectCard in selectCards.Where(condition.Check))
        {
            ISkillAction action = skillActionFactory.CreateAction(skillData.SkillType, new StatChanger2(skillData.AmountData.StatType, 
                SkillAmountCalculatorFactory.Create(skillData.AmountData, selectCard.Stat).Calculate(selectCard.GetStatAmount(skillData.AmountData.StatType))), caster, team);
            if (SkillApplyTargetType.Select == skillApplyTargetType) action.Do(selectCard);
            else action.Do(caster);
        }
    }


    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}