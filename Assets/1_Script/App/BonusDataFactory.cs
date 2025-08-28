using UnityEngine;


[CreateAssetMenu(fileName = "BonusDataFactorySO", menuName = "BP Master/BonusFactorySO")]
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