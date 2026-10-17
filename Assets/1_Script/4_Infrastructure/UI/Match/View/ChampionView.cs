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
        string skillText = skillTextSO.CreateSkillTextBuilder().BuildSkillText(champion.Skill.SkillDatas);
        UpdateChampionAll(new ChampionTextModel(champion.ChampionName, ChampionStatModelPreserter.CreateStatModel(champion.StatData), skillText));
    }

    public void UpdateDisplay(int id) => UpdateDisplay(ChampionDataLoder.GetChampionData(id));

    public void UpdateChampionAll(ChampionTextModel model)
    {
        nameText.text = $"{model.Name}";
        statView.UpdateStat(model.StatModel);
        if (skillText != null)
            skillText.text = model.SkillText;
    }

    public void ClearDisplay()
    {
        nameText.text = "-";
        statView.ClearDisplay();
        if(skillText != null)
            skillText.text = "";
    }
}
