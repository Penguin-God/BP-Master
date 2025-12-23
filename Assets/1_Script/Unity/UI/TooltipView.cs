using TMPro;
using UnityEngine;

public class TooltipView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _contentText;
    RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    // 외부에서 이 함수를 호출해 툴팁을 셋업하고 보여줍니다.
    public void Show(string content, Vector2 size, Vector2 position)
    {
        _contentText.text = content;
        _rectTransform.sizeDelta = size; // UI마다 원하는 크기 적용
        transform.position = position;   // UI가 원하는 고정 위치 적용
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}