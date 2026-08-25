using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_DeckCard : MonoBehaviour, IPointerClickHandler
{
    public CardIdentity Identity { get; private set; }

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject highlightObj;

    private Action<CardIdentity> onClick;
    private Action<CardIdentity> onDoubleClick;

    public void Init(CardIdentity identity, string name, Color cardColor, Action<CardIdentity> onClick, Action<CardIdentity> onDoubleClick)
    {
        Identity = identity;
        nameText.text = name;
        this.onClick = onClick;
        this.onDoubleClick = onDoubleClick;

        // 자신의 Image 컴포넌트 색상을 변경
        if (TryGetComponent<Image>(out var bgImage))
        {
            bgImage.color = cardColor;
        }

        SetFocus(false);
    }

    public void SetFocus(bool isFocused)
    {
        if (highlightObj != null)
            highlightObj.SetActive(isFocused);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
            onDoubleClick?.Invoke(Identity);
        else if (eventData.clickCount == 1)
            onClick?.Invoke(Identity);
    }
}