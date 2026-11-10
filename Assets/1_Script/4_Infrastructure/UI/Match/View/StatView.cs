using TMPro;
using UnityEngine;

public class StatView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;

    void Start() => ClearDisplay();

    public void UpdateStat(ChampionStatData statData) => UpdateStat(ChampionStatModelPreserter.CreateStatModel(statData));

    public void UpdateStat(ChampionStatModel model)
    {
        attackText.text = model.Attack;
        defenseText.text = model.Defense;
        if (speedText != null) speedText.gameObject.SetActive(false);
    }

    public void ClearDisplay()
    {
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        if (speedText != null) speedText.gameObject.SetActive(false);
    }
}
