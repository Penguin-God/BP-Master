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

    void Start() => ClearDisplay();

    ChampionPersenter championPersenter = new ChampionPersenter();
    public void UpdateDisplay(ChampionSO champion)
    {
        var viewModel = championPersenter.CreateViewModel(champion.StatData, champion.TraitData.CreateUI_Data());
        nameText.text = champion.ChampionName;
        attackText.text = viewModel.Attack;
        defenseText.text = viewModel.Defense;
        speedText.text = viewModel.Speed;
    }

    public void UpdateStat(ChampionStatData statData)
    {
        var viewModel = championPersenter.CreateViewModel(statData, default);
        attackText.text = viewModel.Attack;
        defenseText.text = viewModel.Defense;
        speedText.text = viewModel.Speed;
    }

    void ClearDisplay()
    {
        nameText.text = "챔피언 없음";
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        speedText.text = "속도 : -";
    }
}
