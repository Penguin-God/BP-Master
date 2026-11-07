using TMPro;
using UnityEngine;

public class SlotView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] StatView _statView;
    [SerializeField] StatChangeView statChangeView;
    [SerializeField] UI_CardItem cardItem;

    void Start()
    {
        cardItem.ChangeName("-");
        cardItem.ActiveIcon(false);
    }

    public void PickCard(ChampionStatus status, int id)
    {
        _nameText.text = ChampionDataLoder.NameCatalog[id];
        _statView.UpdateStat(status.Stat);
        if (statChangeView != null)
            status.OnStatChanged += (be, af) => statChangeView.ChangeStat(new StatChangeData(be, af));
        cardItem.DrawCard(ChampionDataLoder.GetChampionData(id));
    }
}