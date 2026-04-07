using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SkillAmount
{
    [EnumToggleButtons] [SerializeField] AmountType Type;
    [EnumToggleButtons] [SerializeField] StatType StatType;

    [ShowIf(nameof(Type), AmountType.Value)] [Indent] [SerializeField] 
    int ValueAmount;

    [ShowIf(nameof(Type), AmountType.Percent)] [Indent] [SuffixLabel("X100", overlay: false)]
    [SerializeField] float PercentValue;

    [ShowIf(nameof(Type), AmountType.Fix)] [Indent] [SerializeField] 
    int FixValue;

    public SkillAmountData ToData() => new(Type, StatType, ValueAmount, PercentValue, FixValue);
}