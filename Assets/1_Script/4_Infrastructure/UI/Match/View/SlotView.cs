using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;

    int id;
    ChampionView championFocusView;

    void Start()
    {
        slotButton.onClick.AddListener(DrawTarget);
    }

    public void Init(ChampionView championFocusView) => this.championFocusView = championFocusView;

    public void UpdateChampion(ChampionStatus status, int id)
    {
        this.id = id;
        championView.UpdateChampion(new ChampionTextModel(ChampionDataLoder.NameCatalog[id], status.Stat));
    }

    void DrawTarget() => championFocusView.UpdateDisplay(ChampionDataLoder.GetChampionData(id));
}