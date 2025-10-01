using System.Collections.Generic;
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

    StatPersenter championPersenter = new StatPersenter();
    TraitPersenter traitPersenter = new();
    public void UpdateDisplay(ChampionSO champion) => UpdateDisplay(champion.CreateChampion(), champion.CreateTrait_UI_Datas());

    public void UpdateDisplay(Champion champion, IEnumerable<TraitUI_Data> traitUI_Data)
    {
        UpdateChampion(champion);
        traitText.text = traitPersenter.BuildTraitText(traitUI_Data);
    }

    public void UpdateChampion(Champion champion)
    {
        nameText.text = champion.Name;
        UpdateStat(champion.StatData);
    }

    public void UpdateStat(ChampionStatData statData)
    {
        StatViewModel viewModel = championPersenter.CreateStatViewModel(statData);
        attackText.text = viewModel.Attack;
        defenseText.text = viewModel.Defense;
        speedText.text = viewModel.Speed;
    }

    public void ClearDisplay()
    {
        if (nameText != null)
            nameText.text = "챔피언 없음";
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        speedText.text = "속도 : -";
        if(traitText != null)
            traitText.text = "";
    }
}
