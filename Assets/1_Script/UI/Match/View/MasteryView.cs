using UnityEngine;
using TMPro;

public class MasteryView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blue;
    [SerializeField] TextMeshProUGUI red;
    [SerializeField] GamerRoster masteryData;

    public void ViewMastery(ChampionRepository championRepository)
    {
        MasteryTextBuilder textBuilder = new MasteryTextBuilder(championRepository.NameCatalog);
        blue.text = textBuilder.BuildMasteriesText(masteryData.blues);
        red.text = textBuilder.BuildMasteriesText(masteryData.reds);
    }
}
