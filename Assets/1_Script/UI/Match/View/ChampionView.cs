using TMPro;
using UnityEngine;

public class ChampionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] StatView statView;
    [SerializeField] TextMeshProUGUI skillText;

    void Start() => ClearDisplay();

    readonly SkillTextBuilder skillTextBuilder = new();
    public void UpdateDisplay(ChampionSO champion)
    {
        UpdateChampion(champion);
        skillText.text = skillTextBuilder.BuildSkillText(champion.CreateSkill_UI_Datas());
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
        if(skillText != null)
            skillText.text = "";
    }
}
