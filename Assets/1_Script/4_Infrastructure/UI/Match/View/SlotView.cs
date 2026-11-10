using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] StatView _statView;
    [SerializeField] StatChangeView statChangeView;
    [SerializeField] UI_CardItem cardItem;
    [SerializeField] Image _highlightImage;

    bool _isOverlayHighlight;

    public Image HighlightImage => EnsureHighlightImage();
    public Color CurrentColor => HighlightImage != null ? HighlightImage.color : SlotHighlightPalette.DefaultColor;

    void Start()
    {
        EnsureHighlightImage();
        if (cardItem != null)
        {
            cardItem.ChangeName("-");
            cardItem.ActiveIcon(false);
        }
    }

    public void PickCard(ChampionStatus status, int id)
    {
        _nameText.text = ChampionDataLoder.NameCatalog[id];
        _statView.UpdateStat(status.Stat);
        if (statChangeView != null)
            status.OnStatChanged += (be, af) => statChangeView.ChangeStat(new StatChangeData(be, af));
        cardItem.DrawCard(ChampionDataLoder.GetChampionData(id));
    }

    public void SetHighlight(Color color)
    {
        var image = EnsureHighlightImage();
        if (image != null)
        {
            if (_isOverlayHighlight)
                image.gameObject.SetActive(true);
            image.color = color;
        }
    }

    public void ClearHighlight()
    {
        var image = EnsureHighlightImage();
        if (image != null)
        {
            image.color = SlotHighlightPalette.DefaultColor;
            if (_isOverlayHighlight)
                image.gameObject.SetActive(false);
        }
    }

    Image EnsureHighlightImage()
    {
        if (_highlightImage == null)
            _highlightImage = GetComponent<Image>();

        _isOverlayHighlight = _highlightImage != null && _highlightImage.gameObject != gameObject;
        return _highlightImage;
    }
}
