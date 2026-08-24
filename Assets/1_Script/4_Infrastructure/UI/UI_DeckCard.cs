using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_DeckCard : MonoBehaviour, IPointerClickHandler
{
    public CardIdentity Identity { get; private set; }

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject highlightObj;

    private Action<CardIdentity> onClick;
    private Action<CardIdentity> onDoubleClick;

    public void Init(CardIdentity identity, string name, Action<CardIdentity> onClick, Action<CardIdentity> onDoubleClick)
    {
        Identity = identity;
        nameText.text = name;
        this.onClick = onClick;
        this.onDoubleClick = onDoubleClick;

        SetFocus(false);
    }

    public void SetFocus(bool isFocused) => highlightObj?.SetActive(isFocused);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) onDoubleClick?.Invoke(Identity);
        else if (eventData.clickCount == 1) onClick?.Invoke(Identity);
    }
}