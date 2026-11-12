using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour, ISlotView
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] StatView _statView;
    [SerializeField] StatChangeView statChangeView;
    [SerializeField] UI_CardItem cardItem;
    [SerializeField] Image _highlightImage;

    bool _isOverlayHighlight;
    Coroutine _activeCoroutine;

    public Image HighlightImage => EnsureHighlightImage();
    public Color CurrentColor => HighlightImage != null ? HighlightImage.color : SlotHighlightPalette.DefaultColor;
    public SlotUIState CurrentState { get; private set; } = SlotUIState.Default;
    public bool IsAnimating => _activeCoroutine != null;

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
        status.OnStatChanged += (_, af) => UpdateStatView(af);
        cardItem.DrawCard(ChampionDataLoder.GetChampionData(id));
    }

    public void UpdateStatView(ChampionStatData stat)
    {
        if (_statView != null)
            _statView.UpdateStat(stat);
    }

    public void StopCurrentAnimation()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }
    }

    public void SetState(SlotUIState state)
    {
        StopCurrentAnimation();
        CurrentState = state;

        switch (state)
        {
            case SlotUIState.Default:
                SetHighlight(SlotHighlightPalette.DefaultBorder);
                SetButtonInteractable(true);
                break;

            case SlotUIState.Dead:
                UpdateStatView(new ChampionStatData(0, 0));
                SetHighlight(SlotHighlightPalette.DeadTint);
                SetButtonInteractable(true);
                break;

            case SlotUIState.AttackCasting:
                SetHighlight(SlotHighlightPalette.AttackCaster);
                SetButtonInteractable(false);
                break;

            case SlotUIState.SkillCasting:
                SetHighlight(SlotHighlightPalette.SkillCaster);
                SetButtonInteractable(false);
                break;

            case SlotUIState.Untargetable:
                SetHighlight(SlotHighlightPalette.UntargetableTint);
                SetButtonInteractable(false);
                break;

            case SlotUIState.Targetable:
                SetHighlight(SlotHighlightPalette.TargetableBorder);
                SetButtonInteractable(true);
                if (isActiveAndEnabled)
                    _activeCoroutine = StartCoroutine(Co_PulseBorder());
                break;

            case SlotUIState.AttackHit:
                SetHighlight(SlotHighlightPalette.AttackHit);
                SetButtonInteractable(false);
                if (isActiveAndEnabled)
                    _activeCoroutine = StartCoroutine(Co_FlashOverlay(SlotHighlightPalette.AttackHit));
                break;

            case SlotUIState.SkillImpact:
                SetHighlight(SlotHighlightPalette.SkillImpact);
                SetButtonInteractable(false);
                if (isActiveAndEnabled)
                    _activeCoroutine = StartCoroutine(Co_FlashOverlay(SlotHighlightPalette.SkillImpact));
                break;
        }
    }

    IEnumerator Co_PulseBorder()
    {
        Color bright = SlotHighlightPalette.TargetableBorder;
        Color dim = new Color(bright.r * 0.55f, bright.g * 0.55f, bright.b * 0.55f, 1f);
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;
            float t = (Mathf.Sin((elapsed / SlotHighlightPalette.PulsePeriod) * Mathf.PI * 2f) + 1f) * 0.5f;
            SetHighlight(Color.Lerp(dim, bright, t));
            yield return null;
        }
    }

    IEnumerator Co_FlashOverlay(Color flashColor)
    {
        Color endColor = SlotHighlightPalette.DefaultBorder;
        float elapsed = 0f;

        while (elapsed < SlotHighlightPalette.FlashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / SlotHighlightPalette.FlashDuration);
            SetHighlight(Color.Lerp(flashColor, endColor, t));
            yield return null;
        }

        _activeCoroutine = null;
    }

    void SetButtonInteractable(bool interactable)
    {
        var button = GetComponent<Button>();
        if (button != null)
            button.interactable = interactable;
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
