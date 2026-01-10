using UnityEngine;

[CreateAssetMenu(fileName = "BonusDataFactorySO", menuName = "BP Master/BonusFactorySO")]
public class BonusDataFactory : ScriptableObject
{
    [SerializeField] BonusDataSO attackBonus;
    public BonusDataSO AttackBonus => attackBonus;

    [SerializeField] BonusDataSO defenseBonus;
    public BonusDataSO DefenseBonus => defenseBonus;

    [SerializeField] BonusDataSO speedBonus;
    public BonusDataSO SpeedBonus => speedBonus;

    public TeamBonusCalculator TeamBonus => new TeamBonusCalculator(attackBonus.Bonus, defenseBonus.Bonus, speedBonus.Bonus);
}