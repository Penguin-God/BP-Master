using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChampionView : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;

    Champion currentChampion;

    public void SetChampion(Champion champion)
    {
        currentChampion = champion;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (currentChampion == null)
        {
            ClearDisplay();
            return;
        }

        nameText.text = currentChampion.Name;
        attackText.text = $"공격 : {currentChampion.StatData.Attack}";
        defenseText.text = $"방어 : {currentChampion.StatData.Defense}";
        speedText.text = $"속도 : {currentChampion.StatData.Speed}";
    }

    void ClearDisplay()
    {
        nameText.text = "챔피언 없음";
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        speedText.text = "속도 : -";
    }

    void Start()
    {
        if (currentChampion == null)
            ClearDisplay();
    }
}
