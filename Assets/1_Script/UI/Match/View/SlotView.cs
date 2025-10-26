using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;
    [SerializeField] TextMeshProUGUI masteryText;

    ChampionRepository championManager;
    int id;
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

    void UpdateMasteryText(ProGamer gamer) => masteryText.text = new MasteryTextBuilder(championManager.Catalog).BuildMasteriesText(gamer.AllMasteries);

    public void UpdateChampion(int id)
    {
        this.id = id;
        championView.UpdateChampion(championManager.GetChampionData(id));
    }

    void DrawTarget() => championFocusView.UpdateDisplay(championManager.GetChampionData(id));
}
