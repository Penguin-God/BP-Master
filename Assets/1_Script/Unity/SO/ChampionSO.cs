using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TraitDataConfig
{
    [Header("범위")]
    [SerializeField] Side targetSide;
    [SerializeField] TargetRange range;

    [Header("액션")]
    [SerializeField] TraitType traitType;
    [SerializeField] int amount;

    [Header("조건")]
    [SerializeField] TraitConditionType conditionType;
    [SerializeField] int threshold;

    public TraitData CreateTraitData() => new TraitData(traitType, amount, new TraitConditionData(conditionType, threshold), new TraitTargetRule(targetSide, range));
    public TraitUI_Data CreateUI_Data() => new TraitUI_Data(traitType, targetSide, range, amount, conditionType, threshold);
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

    [Header("특성")]
    [SerializeField] TraitDataConfig[] traitDatas;
    public Champion CreateChampion() => new Champion(id, championName, StatData, traitDatas.Select(x => x.CreateTraitData()));
    public IEnumerable<TraitUI_Data> CreateTrait_UI_Datas() => traitDatas.Select(x => x.CreateUI_Data());
}
