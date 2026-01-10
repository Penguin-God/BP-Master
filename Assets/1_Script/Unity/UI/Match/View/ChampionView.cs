using TMPro;
using UnityEngine;

public class ChampionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] StatView statView;
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] SkillTextSO skillTextSO;
    void Start() => ClearDisplay();

    readonly ChampionStatusTextBuilder statusTextBuilder = new();
    public void UpdateDisplay(ChampionSO champion)
    {
        UpdateChampion(champion.CreateChampionModel());
        skillText.text = skillTextSO.CreateSkillTextBuilder().BuildSkillText(champion.CreateSkill_UI_Datas());
    }

    public void UpdateChampion(ChampionModel model)
    {
        nameText.text = $"{model.Name}({statusTextBuilder.BuildTraitText(model.TraitType)})";
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
