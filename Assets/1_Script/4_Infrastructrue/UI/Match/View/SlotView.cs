using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;

    ChampionRepository championManager;
    int id;
    ChampionView championFocusView;

    void Start()
    {
        slotButton.onClick.AddListener(DrawTarget);
    }

    public void Init(ChampionView championFocusView, ChampionRepository championManager)
    {
        this.championFocusView = championFocusView;
        this.championManager = championManager;
    }

    public void UpdateChampion(ChampionStatus status, int id)
    {
        this.id = id;
        championView.UpdateChampion(new ChampionModel(championManager.GetChampionName(id), status.Stat, status.TraitType));
    }

    void DrawTarget() => championFocusView.UpdateDisplay(championManager.GetChampionData(id));
}
