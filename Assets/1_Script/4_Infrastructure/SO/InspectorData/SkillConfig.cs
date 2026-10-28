using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SkillActionConfig
{
    [SerializeField] SkillType skillType;
    [SerializeField] SkillAmount skillAmount;

    public SkillType SkillType => skillType;
    public SkillAmount SkillAmount => skillAmount;
    public SkillAmountData GetValueData() => skillAmount.ToData();

    public SkillActionConfig(SkillType skillType, SkillAmount skillAmount)
    {
        this.skillType = skillType;
        this.skillAmount = skillAmount;
    }
}

[Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [EnumToggleButtons][SerializeField] Side targetSide;
    [EnumToggleButtons][SerializeField] TargetRange range;
    public CardSelectRule Rule => new CardSelectRule(targetSide, range);

    [EnumToggleButtons][SerializeField] SkillApplyType applyType;
    [EnumToggleButtons][SerializeField] SkillValueType valueType;
    [EnumToggleButtons][SerializeField] SkillUseTimingType timingType;

    [Header("액션")]
    [SerializeField] SkillType skillType = SkillType.StatChanger;
    [SerializeField] SkillAmount skillAmount;
    public SkillAmountData GetValueData() => skillAmount.ToData();

    [Header("복수 액션")]
    [SerializeField] SkillActionConfig[] actions;
    public IReadOnlyList<SkillActionConfig> Actions => actions;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    public SkillConditionData Condition => new SkillConditionData(conditionType, threshold, conditionCheckerType);

    [SerializeField] bool isSpecialText;
    [ShowIf(nameof(isSpecialText))]
    [SerializeField] string specialTemplate;

    public SkillData CreateSkillData() => new SkillData(skillType, CalculatorFactory.Create(skillAmount.ToData()), SkillCondtionFactory.CreateCondition(Condition), Rule, applyType, valueType, skillAmount.ToData().StatType, timingType);
    public SkillViewModel CreateViewModel() => new SkillViewModel(skillType, GetValueData(), Condition, Rule, specialTemplate?.Trim());

    public IEnumerable<SkillData> CreateSkillDatas()
    {
        if (actions != null && actions.Length > 0)
        {
            return actions.Select(action => new SkillData(
                action.SkillType,
                CalculatorFactory.Create(action.GetValueData()),
                SkillCondtionFactory.CreateCondition(Condition),
                Rule,
                applyType,
                valueType,
                action.GetValueData().StatType,
                timingType
            ));
        }

        return new[] { CreateSkillData() };
    }

    public IEnumerable<SkillViewModel> CreateViewModels()
    {
        if (actions != null && actions.Length > 0)
        {
            return actions.Select(action => new SkillViewModel(
                action.SkillType,
                action.GetValueData(),
                Condition,
                Rule,
                specialTemplate?.Trim()
            ));
        }

        return new[] { CreateViewModel() };
    }
}