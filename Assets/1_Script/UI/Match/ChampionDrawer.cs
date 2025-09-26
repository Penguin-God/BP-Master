using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChampionDrawer : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] GameObject championBtn;
    [SerializeField] Transform content;

    public void DrawChampionButtons(UnityAction<ChampionSO, Button> onclick)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var data in championManager.AllChampion)
        {
            var btn = Instantiate(championBtn, content).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = data.ChampionName;
            btn.onClick.AddListener(() => onclick(data, btn));
        }
    }

    public void HideView() => gameObject.SetActive(false);

    // 혹시 다시 마우스 올리는 기능을 쓰지 않을까하는 기대감에 남겨둠
    //public void DrawChampionButtons(UnityAction<ChampionSO> onclick, UnityAction<ChampionSO> pointerEnter, UnityAction<ChampionSO> pinterExit)
    //{
    //    foreach (Transform child in transform)
    //        Destroy(child.gameObject);

    //    foreach (var data in championManager.AllChampion)
    //    {
    //        var championSO = data; // ★ 클로저 안전용 로컬 복사
    //        var btn = Instantiate(championBtn, transform).GetComponent<Button>();
    //        btn.GetComponentInChildren<TextMeshProUGUI>().text = championSO.ChampionName;

    //        btn.onClick.AddListener(() => onclick?.Invoke(championSO));

    //        var trigger = btn.gameObject.GetComponent<EventTrigger>();
    //        if (trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();

    //        var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
    //        enter.callback.AddListener(_ => pointerEnter?.Invoke(championSO));
    //        trigger.triggers.Add(enter);

    //        var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
    //        exit.callback.AddListener(_ => pinterExit?.Invoke(championSO));
    //        trigger.triggers.Add(exit);
    //    }
    //}
}
