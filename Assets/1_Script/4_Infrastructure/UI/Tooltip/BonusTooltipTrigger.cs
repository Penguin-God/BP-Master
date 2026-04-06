using UnityEngine;

public class BonusTooltipTrigger : TooltipTrigger
{
    [SerializeField] TeamBonusDataSO bonusData;
    protected override string BuildText() => new BonusTextBulider().BuildBonusAllText(bonusData.AttackBonus.BonusDatas, bonusData.DefenseBonus.BonusDatas, bonusData.SpeedBonus.BonusDatas);
}
