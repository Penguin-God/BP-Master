using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

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

    public void Run2(Skill skill, ChampionStatus caster, IEnumerable<ChampionStatus> selectTargets, Team team)
    {
        if (skill.IsEmpty) return;

        foreach (var skillData in skill.SkillDatas)
        {
            IChampionCondition condition = SkillCondtionFactory.CreateCondition(skillData.ConditionData, caster.Stat);
            ExecuteSkill(condition, selectTargets, skillData, caster);
        }
    }

    void ExecuteSkill(IChampionCondition condition, IEnumerable<ChampionStatus> selectCards, SkillData skillData, ChampionStatus caster)
    {
        foreach (var selectCard in selectCards.Where(condition.Check))
        {
            int value = 0;

            switch (skillData.SkillValueType)
            {
                case SkillValueType.SelfRef: value = caster.GetStatAmount(skillData.AmountData.StatType); break;
                case SkillValueType.SelectRef: value = selectCard.GetStatAmount(skillData.AmountData.StatType); break;
            }

            int applyValue = SkillAmountCalculatorFactory.Create(skillData.AmountData, selectCard.Stat).Calculate(value);

            if (SkillApplyType.Select == skillData.SkillApplyType) selectCard.ChangeStat(applyValue, skillData.AmountData.StatType);
            else caster.ChangeStat(applyValue, skillData.AmountData.StatType);
        }
    }

    void ExecuteSkill(ISkillAction action, IChampionCondition condition, IEnumerable<ChampionStatus> targets)
        => targets
            .Where(condition.Check)
            .ForEach(target => action.Do(target));
}