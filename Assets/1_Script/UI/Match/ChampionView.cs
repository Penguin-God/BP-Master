using TMPro;
using UnityEngine;

public class ChampionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI traitText;

    void Start() => ClearDisplay();

    ChampionPersenter championPersenter = new ChampionPersenter();
    public void UpdateDisplay(ChampionSO champion)
    {
        UpdateChampion(champion.CreateChampion());
        traitText.text = championPersenter.CreateViewModel(champion.StatData, champion.TraitData.CreateUI_Data()).Trait;
    }

    public void UpdateChampion(Champion champion)
    {
        nameText.text = champion.Name;
        UpdateStat(champion.StatData);
    }

    public void UpdateStat(ChampionStatData statData)
    {
        var viewModel = championPersenter.CreateViewModel(statData, default);
        attackText.text = viewModel.Attack;
        defenseText.text = viewModel.Defense;
        speedText.text = viewModel.Speed;
    }

    public void ClearDisplay()
    {
        nameText.text = "챔피언 없음";
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        speedText.text = "속도 : -";
        if(traitText != null)
            traitText.text = "";
    }
}
