using TMPro;
using UnityEngine;

public class ChampionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] StatView statView;
    [SerializeField] TextMeshProUGUI traitText;

    void Start() => ClearDisplay();

    TraitTextBuilder traitPersenter = new();
    public void UpdateDisplay(ChampionSO champion)
    {
        UpdateChampion(champion);
        traitText.text = traitPersenter.BuildTraitText(champion.CreateTrait_UI_Datas());
    }
    public void UpdateChampion(ChampionSO champion)
    {
        nameText.text = champion.ChampionName;
        statView.UpdateStat(champion.StatData);
    }

    public void ClearDisplay()
    {
        nameText.text = "-";
        statView.ClearDisplay();
        if(traitText != null)
            traitText.text = "";
    }
}
