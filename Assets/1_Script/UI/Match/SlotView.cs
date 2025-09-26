using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;
    [SerializeField] TextMeshProUGUI masteryText;

    ChampionRepository championManager;
    Champion traickingTarget;
    ChampionView championFocusView;

    void Start()
    {
        slotButton.onClick.AddListener(DrawTarget);
    }

    public void Init(ChampionView championFocusView, ChampionRepository championManager, ProGamer gamer)
    {
        this.championFocusView = championFocusView;
        this.championManager = championManager;
        UpdateMasteryText(gamer);
    }

    void UpdateMasteryText(ProGamer gamer) => masteryText.text = new MasteryPersenter(championManager.Catalog).BuildMasteriesText(gamer.AllMasteries);

    public void UpdateChampion(Champion target)
    {
        traickingTarget = target;
        championView.UpdateChampion(target);
    }

    void DrawTarget() => championFocusView.UpdateDisplay(traickingTarget, championManager.GetChampionData(traickingTarget.Id).TraitData.CreateUI_Data());
}
