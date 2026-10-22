using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [EnumToggleButtons] [SerializeField] Side targetSide;
    [EnumToggleButtons] [SerializeField] TargetRange range;
    CardSelectRule Rule => new CardSelectRule(targetSide, range);

    [EnumToggleButtons][SerializeField] SkillApplyType applyType;
    [EnumToggleButtons][SerializeField] SkillValueType valueType;

    [Header("액션")]
    [SerializeField] SkillType skillType;
    [SerializeField] SkillAmount skillAmount;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    SkillConditionData Condition => new SkillConditionData(conditionType, threshold, conditionCheckerType);

    public SkillData CreateSkillData() => new SkillData(skillType, CalculatorFactory.Create(skillAmount.ToData()), SkillCondtionFactory.CreateCondition(Condition), Rule, applyType, valueType, skillAmount.ToData().StatType);
}

[CreateAssetMenu(fileName = "ChampionSO", menuName = "Data/Champion")]
public class ChampionSO : SerializedScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string championName;
    public string ChampionName => championName;

    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    public ChampionStatData StatData => new ChampionStatData(attack, defense, speed);

    [Header("스킬")]
    [SerializeField] TraitDataConfig[] skillDatas;
    public Skill Skill => new Skill(skillDatas.Select(x => x.CreateSkillData()));
    public ChampionStatus CreateStatus() => new ChampionStatus(StatData);
    public Champion CreateChampion() => new Champion(Id, Skill, CreateStatus());

    [SerializeField] [PreviewField(70)] Sprite icon;
    public Sprite Icon => icon;
}