using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct BonusData
{
    public int Threshold;
    public int Bonus;
}

[CreateAssetMenu(fileName = "BonusDataFactorySO", menuName = "Scriptable Objects/BonusDataSO")]
public class BonusDataFactory : ScriptableObject
{
    [SerializeField] BonusDataSO champAttackBonus;
    [SerializeField] BonusDataSO champDefenseBonus;

    [SerializeField] BonusDataSO attackBonus;
    [SerializeField] BonusDataSO defenseBonus;
    [SerializeField] BonusDataSO rangeBonus;
    [SerializeField] BonusDataSO speedBonus;

    public ChampionBonusCalculator ChampionBonus => new ChampionBonusCalculator(champAttackBonus.Bonus, champDefenseBonus.Bonus);
    public TeamBonusCalculator TeamBonus => new TeamBonusCalculator(attackBonus.Bonus, defenseBonus.Bonus, rangeBonus.Bonus, speedBonus.Bonus);
}

[CreateAssetMenu(fileName = "BonusDataSO", menuName = "Scriptable Objects/BonusDataSO")]
public class BonusDataSO : ScriptableObject
{
    [SerializeField] BonusData[] bonusDatas;
    public BonusCalculator Bonus => new BonusCalculator(new SortedDictionary<int, int>(bonusDatas.ToDictionary(x => x.Threshold, x => x.Bonus)));
}
