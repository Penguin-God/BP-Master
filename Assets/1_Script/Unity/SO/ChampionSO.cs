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

    public TraitData CreateTraitData() => new TraitData(traitType, amount, conditionType, threshold, new TraitTargetRule(targetSide, range));
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
    [SerializeField] TraitDataConfig traitData;
    public TraitDataConfig TraitData => traitData;
    public Champion CreateChampion() => new Champion(id, championName, StatData, traitData.CreateTraitData());
}
