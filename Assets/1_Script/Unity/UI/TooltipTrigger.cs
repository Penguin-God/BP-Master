using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Configuration")]
    [SerializeField] TooltipView tooltip;
    [SerializeField] Vector2 tooltipSize = new Vector2(300, 100); // 툴팁 크기
    [SerializeField] Vector3 positionOffset = new Vector3(50, 50, 0); // 틀팁 위치

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 targetPos = transform.position + positionOffset;
        tooltip.Show("AAAAAAAAAAAAAAAAAA", tooltipSize, targetPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}