using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] BonusDataFactory bonusData;
    [SerializeField] TextMeshProUGUI bonusInfo;

    void Start()
    {
        bonusInfo.text = new BonusPresenter().BuildBonusAllText(bonusData.AttackBonus.BonusDatas, bonusData.DefenseBonus.BonusDatas, bonusData.SpeedBonus.BonusDatas);
    }
}
