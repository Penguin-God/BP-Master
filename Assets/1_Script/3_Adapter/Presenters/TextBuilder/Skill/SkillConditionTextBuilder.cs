public class SkillConditionTextBuilder
{
    public string BuildConditionText(SkillConditionData conditionData) => conditionData.ConditionType switch
    {
        ConditionType.None => "",
        ConditionType.Threshold => BuildThresholdText(conditionData.StatType, conditionData.Threshold),
        ConditionType.Compare => BuildCompareText(conditionData.StatType),
        ConditionType.Trait => BuildTriatText(conditionData.TraitType),
    };

    string BuildThresholdText(StatConditionType conditionType, int threshold) => conditionType switch
    {
        StatConditionType.None => "",
        StatConditionType.AttackAtLeast => $"공격력 {threshold} 이상인",
        StatConditionType.AttackBelow => $"공격력 {threshold} 이하인",
        StatConditionType.DefenseAtLeast => $"방어력 {threshold} 이상인",
        StatConditionType.DefenseBelow => $"방어력 {threshold} 이하인",
        StatConditionType.SpeedAtLeast => $"속도 {threshold} 이상인",
        StatConditionType.SpeedBelow => $"속도 {threshold} 이하인",
        _ => ""
    };

    string BuildCompareText(StatConditionType conditionType) => conditionType switch
    {
        StatConditionType.None => "",
        StatConditionType.AttackAtLeast => $"공격력이 자신보다 높은",
        StatConditionType.AttackBelow => $"공격력이 자신보다 낮은",
        StatConditionType.DefenseAtLeast => $"방어력이 자신보다 높은",
        StatConditionType.DefenseBelow => $"방어력이 자신보다 낮은",
        StatConditionType.SpeedAtLeast => $"속도가 자신보다 높은",
        StatConditionType.SpeedBelow => $"속도가 자신보다 낮은",
        _ => ""
    };

    string BuildTriatText(TraitType traitType) => $"특성이 {new ChampionStatusTextBuilder().BuildTraitText(traitType)}인";
}