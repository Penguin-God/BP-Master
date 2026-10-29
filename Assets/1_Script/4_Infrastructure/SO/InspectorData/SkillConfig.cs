using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MultiConfig
{
    [SerializeField] SkillAmount skillAmount;

    public SkillAmountData GetValueData() => skillAmount.ToData();
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

    [Header("복수 액션")]
    [SerializeField] MultiConfig[] actions;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    public SkillConditionData Condition => new SkillConditionData(conditionType, threshold, conditionCheckerType);

    [SerializeField] bool isSpecialText;
    [ShowIf(nameof(isSpecialText))]
    [SerializeField] string specialTemplate;

    public SkillData CreateSkillData(SkillAmountData skillAmountData) => new SkillData(skillType, CalculatorFactory.Create(skillAmountData), SkillCondtionFactory.CreateCondition(Condition), Rule, applyType, valueType, skillAmountData.StatType, timingType);
    public SkillViewModel CreateViewModel(SkillAmountData skillAmountData) => new SkillViewModel(skillType, skillAmountData, Condition, Rule, specialTemplate?.Trim());

    bool ExistSkill => actions != null && actions.Length > 0;

    public IEnumerable<SkillData> CreateSkillDatas()
    {
        if (ExistSkill) return actions.Select(action => CreateSkillData(action.GetValueData()));

        return Array.Empty<SkillData>();
    }

    public IEnumerable<SkillViewModel> CreateViewModels()
    {
        if(isSpecialText && string.IsNullOrEmpty(specialTemplate)) return Array.Empty<SkillViewModel>();

        if (ExistSkill) return actions.Select(action => CreateViewModel(action.GetValueData()));

        return Array.Empty<SkillViewModel>();
    }
}