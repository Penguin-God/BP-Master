using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;

    ChampionRepository championManager;
    int id;
    ChampionView championFocusView;
    ChampionStatus status;

    void Start()
    {
        slotButton.onClick.AddListener(DrawTarget);
    }

    public void Init(ChampionView championFocusView, ChampionRepository championManager)
    {
        this.championFocusView = championFocusView;
        this.championManager = championManager;
    }

    public void UpdateChampion(int id)
    {
        this.id = id;
        championView.UpdateChampion(championManager.GetChampionData(id).CreateChampionModel());
    }

    public void UpdateChampion(ChampionStatus status, int id)
    {
        this.status = status;
        championView.UpdateChampion(championManager.GetChampionData(id).CreateChampionModel());
    }

    void DrawTarget() => championFocusView.UpdateDisplay(championManager.GetChampionData(id));
}
