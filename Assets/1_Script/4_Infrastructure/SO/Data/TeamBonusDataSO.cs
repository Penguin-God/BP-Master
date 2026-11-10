using UnityEngine;

[CreateAssetMenu(fileName = "BonusDataFactorySO", menuName = "Data/TeamBonus")]
public class TeamBonusDataSO : ScriptableObject
{
    [SerializeField] BonusDataSO attackBonus;
    public BonusDataSO AttackBonus => attackBonus;

    [SerializeField] BonusDataSO defenseBonus;
    public BonusDataSO DefenseBonus => defenseBonus;

    public TeamBonusCalculator TeamBonus => new TeamBonusCalculator(attackBonus.Bonus, defenseBonus.Bonus);

    public TeamBonusCalculator CreateTeamBonusCalculator() => new TeamBonusCalculator(attackBonus.Bonus, defenseBonus.Bonus);
}
