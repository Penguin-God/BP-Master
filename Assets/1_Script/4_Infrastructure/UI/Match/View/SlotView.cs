using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] StatView _statView;
    [SerializeField] Button slotButton;

    int id;
    ChampionView championFocusView;

    void Start()
    {
        slotButton.onClick.AddListener(DrawTarget);
        void DrawTarget() => championFocusView.UpdateDisplay(ChampionDataLoder.GetChampionData(id));
    }

    public void Init(ChampionView championFocusView)
    {
        this.championFocusView = championFocusView;

    }

    public void UpdateChampion(ChampionStatus status, int id)
    {
        this.id = id;
        _nameText.text = ChampionDataLoder.NameCatalog[id];
        _statView.UpdateStat(status.Stat);
    }
}