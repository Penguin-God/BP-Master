using UnityEngine;
using UnityEngine.EventSystems;

public enum TooltipDir { Top, Bottom, Left, Right }

public abstract class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipView tooltip;
    [SerializeField] Vector2 tooltipSize = new Vector2(300, 100);

    [SerializeField] TooltipDir direction;
    [SerializeField] float spacing = 10f;

    protected abstract string BuildText();

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(BuildText(), tooltipSize, CalculatePosition());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }

    // 좌표 계산 핵심 로직 (피벗/앵커 무시 버전)
    private Vector2 CalculatePosition()
    {
        // 1. 코너 좌표 가져오기
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);

        float tipWidth = tooltipSize.x * tooltip.transform.lossyScale.x;
        float tipHeight = tooltipSize.y * tooltip.transform.lossyScale.y;
        float scaledSpacing = spacing * transform.lossyScale.x;

        // 중앙점 및 상하좌우 좌표 계산
        float myCenterY = (corners[0].y + corners[1].y) / 2f;
        float myCenterX = (corners[0].x + corners[3].x) / 2f;

        float myTop = corners[1].y;    // 내 머리끝 (World Y)
        float myBottom = corners[0].y; // 내 발끝
        float myLeft = corners[0].x;
        float myRight = corners[3].x;

        Vector2 finalPos = Vector2.zero;

        switch (direction)
        {
            case TooltipDir.Top:
                finalPos.x = myCenterX;
                finalPos.y = myTop + scaledSpacing + (tipHeight / 2);
                break;

            case TooltipDir.Bottom:
                finalPos.x = myCenterX;
                finalPos.y = myBottom - scaledSpacing - (tipHeight / 2);
                break;

            case TooltipDir.Left:
                finalPos.x = myLeft - scaledSpacing - (tipWidth / 2);

                // [변경] 중앙 정렬 -> 상단 정렬 (Top Alignment)
                // 내 머리끝 위치에서 툴팁의 절반만큼 내려오면, 툴팁의 머리와 내 머리가 맞춰짐
                finalPos.y = myTop - (tipHeight / 2);
                break;

            case TooltipDir.Right:
                finalPos.x = myRight + scaledSpacing + (tipWidth / 2);

                // [변경] 중앙 정렬 -> 상단 정렬 (Top Alignment)
                finalPos.y = myTop - (tipHeight / 2);
                break;
        }

        return finalPos;
    }
}