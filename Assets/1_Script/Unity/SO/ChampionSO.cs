using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [SerializeField] Side targetSide;
    [SerializeField] TargetRange range;
    TraitTargetRule Rule => new TraitTargetRule(targetSide, range);

    [Header("액션")]
    [SerializeField] SkillType traitType;
    [SerializeField] int amount;

    [Header("조건")]
    [SerializeField] StatConditionType conditionType;
    [SerializeField] int threshold;
    [SerializeField] ConditionType conditionCheckerType;
    SkillConditionData Condition => new SkillConditionData(conditionType, threshold, conditionCheckerType);

    public SkillData CreateTraitData() => new SkillData(traitType, amount, Condition, Rule);
    public TraitUI_Data CreateUI_Data() => new TraitUI_Data(traitType, amount, Condition, Rule);
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
    [SerializeField] TraitDataConfig[] traitDatas;
    public Champion CreateChampion() => new Champion(id, championName, StatData, traitDatas.Select(x => x.CreateTraitData()));
    public ChampionStatus CreateStatus() => new ChampionStatus(StatData, traitType);
    public IEnumerable<TraitUI_Data> CreateTrait_UI_Datas() => traitDatas.Select(x => x.CreateUI_Data());

    [Header("특성")]
    [SerializeField] TraitType traitType;
}
