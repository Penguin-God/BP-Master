using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_DeckCard : MonoBehaviour, IPointerClickHandler
{
    public int CardId { get; private set; }

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject highlightObj; // 포커스 시 켜질 테두리나 배경 이미지

    private Action<int> onClick;
    private Action<int> onDoubleClick;

    public void Init(int id, string name, Action<int> onClick, Action<int> onDoubleClick)
    {
        CardId = id;
        nameText.text = name;
        this.onClick = onClick;
        this.onDoubleClick = onDoubleClick;

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
        {
            onDoubleClick?.Invoke(CardId);
        }
        else if (eventData.clickCount == 1)
        {
            onClick?.Invoke(CardId);
        }
    }
}