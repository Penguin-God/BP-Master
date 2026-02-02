using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [EnumToggleButtons] [SerializeField] Side targetSide;
    [EnumToggleButtons][SerializeField] TargetRange range;
    SkillTargetRule Rule => new SkillTargetRule(targetSide, range);

    [Header("액션")]
    [SerializeField] SkillType skillType;
    [SerializeField] SkillAmount skillAmount;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    SkillConditionData Condition => new SkillConditionData(conditionType, threshold, conditionCheckerType);

    public SkillData CreateSkillData() => new SkillData(skillType, skillAmount.ToData(), Condition, Rule);
    public SkillUI_Data CreateUI_Data() => new SkillUI_Data(CreateSkillData());
}

public readonly struct ChampionModel
{
    public readonly string Name;
    public readonly ChampionStatData Stat;

    public ChampionModel(string name, ChampionStatData stat)
    {
        Name = name;
        Stat = stat;
    }
}

[CreateAssetMenu(fileName = "ChampionSO", menuName = "BP Master/ChampionSO")]
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
    public ChampionModel CreateChampionModel() => new ChampionModel(championName, StatData);
    public IEnumerable<SkillUI_Data> CreateSkill_UI_Datas() => skillDatas.Select(x => x.CreateUI_Data());

    public Champion CreateChampion() => new Champion(Id, Skill, CreateStatus());
}