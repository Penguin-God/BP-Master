using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Configuration")]
    [SerializeField] Vector2 _tooltipSize = new Vector2(300, 100); // 툴팁 크기
    [SerializeField] Vector3 _positionOffset = new Vector3(50, 50, 0); // 틀팁 위치

    string _content;

    Action<string, Vector2, Vector2> _onShowTooltip;
    Action _onHideTooltip;

    public void Init(Action<string, Vector2, Vector2> onShow, Action onHide)
    {
        _onShowTooltip = onShow;
        _onHideTooltip = onHide;
    }

    // 내용물 업데이트 (Presenter 등에서 호출)
    public void SetContent(string content)
    {
        _content = content;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_content) || _onShowTooltip == null) return;

        // "고정된 위치" 계산: 현재 UI 위치 + 오프셋
        Vector2 targetPos = transform.position + _positionOffset;

        // Controller에게 "나 보여줘!" 요청
        _onShowTooltip.Invoke(_content, _tooltipSize, targetPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onHideTooltip?.Invoke();
    }

    // 혹시 오브젝트가 꺼지면 툴팁도 꺼주자 (안전장치)
    private void OnDisable()
    {
        _onHideTooltip?.Invoke();
    }
}