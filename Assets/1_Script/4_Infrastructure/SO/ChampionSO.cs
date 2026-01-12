using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SkillAmount
{
    public AmountType Type;
    public int ValueAmount;
    public float PercentValue;
    public int FixValue;

    public SkillAmountData ToData() => new SkillAmountData(Type, ValueAmount, PercentValue, FixValue);
}

[System.Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [SerializeField] Side targetSide;
    [SerializeField] TargetRange range;
    SkillTargetRule Rule => new SkillTargetRule(targetSide, range);

    [Header("액션")]
    [SerializeField] SkillType traitType;
    [SerializeField] SkillAmount skillAmount;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    [SerializeField] TraitType targetTrait;
    SkillConditionData Condition => new SkillConditionData(conditionType, threshold, targetTrait, conditionCheckerType);

    public SkillData CreateTraitData() => new SkillData(traitType, skillAmount.ToData(), Condition, Rule);
    public SkillUI_Data CreateUI_Data() => new SkillUI_Data(CreateTraitData());
}

public readonly struct ChampionModel
{
    public readonly string Name;
    public readonly ChampionStatData Stat;
    public readonly TraitType TraitType;

    public ChampionModel(string name, ChampionStatData stat, TraitType traitType)
    {
        Name = name;
        Stat = stat;
        TraitType = traitType;
    }
}

[CreateAssetMenu(fileName = "ChampionSO", menuName = "BP Master/ChampionSO")]
public class ChampionSO : ScriptableObject
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
    public Skill Skill => new Skill(skillDatas.Select(x => x.CreateTraitData()));
    public ChampionStatus CreateStatus() => new ChampionStatus(StatData, traitType);
    public ChampionModel CreateChampionModel() => new ChampionModel(championName, StatData, traitType);
    public IEnumerable<SkillUI_Data> CreateSkill_UI_Datas() => skillDatas.Select(x => x.CreateUI_Data());

    [Header("특성")]
    [SerializeField] TraitType traitType;

    public Champion CreateChampion() => new Champion(Id, Skill, CreateStatus());
}