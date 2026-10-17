using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] StatView _statView;
    [SerializeField] Button slotButton;
    [SerializeField] StatChangeView statChangeView;

    public void PickCard(ChampionStatus status, int id)
    {
        _nameText.text = ChampionDataLoder.NameCatalog[id];
        _statView.UpdateStat(status.Stat);
        if (statChangeView != null)
            status.OnStatChanged += (be, af) => statChangeView.ChangeStat(new StatChangeData(be, af));
    }
}