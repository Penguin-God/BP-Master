using TMPro;
using UnityEngine;

public class ChampionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] StatView statView;
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] SkillTextSO skillTextSO;
    void Start() => ClearDisplay();

    public void UpdateDisplay(ChampionSO champion)
    {
        UpdateChampion(champion.CreateChampionModel());
        skillText.text = skillTextSO.CreateSkillTextBuilder().BuildSkillText(champion.Skill.SkillDatas);
    }

    public void UpdateChampion(ChampionTextModel model)
    {
        nameText.text = $"{model.Name}";
        statView.UpdateStat(model.Stat);
    }

    public void ClearDisplay()
    {
        nameText.text = "-";
        statView.ClearDisplay();
        if(skillText != null)
            skillText.text = "";
    }
}
