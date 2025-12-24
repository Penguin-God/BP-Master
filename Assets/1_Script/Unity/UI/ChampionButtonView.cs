using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChampionButtonView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] GameObject championBtn;
    [SerializeField] Transform content;

    IEnumerable<Button> buttons;
    public IEnumerable<Button> Buttons => buttons;

    public void CreateButtons() => buttons = new ChampionButtonCreator().DrawChampionButtons(content, championManager.AllChampion, championBtn);

    public Button GetButton(int id) => buttons.First(x => x.GetComponent<ChampionIdentify>().Id == id);
    public void InActiveButton(int id)
    {
        ButtonUtil.InActiveButton(GetButton(id));
        GetButton(id).GetComponentInChildren<TextMeshProUGUI>().color = new Color32(60, 60, 60, 255);
    }
    public void AddEvent(UnityAction<ChampionIdentify> action)
    {
        foreach (var btn in buttons)
            btn.onClick.AddListener(() => action(btn.GetComponent<ChampionIdentify>()));
    }

    public void HideView() => gameObject.SetActive(false);
}



public class ChampionButtonCreator
{
    public IEnumerable<Button> DrawChampionButtons(Transform parent, IEnumerable<ChampionSO> champions, GameObject btnPrefab)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);

        var result = new List<Button>();
        foreach (var data in champions)
        {
            var btn = Object.Instantiate(btnPrefab, parent).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = data.ChampionName;
            btn.GetOrAddComponent<ChampionIdentify>().Id = data.Id;
            result.Add(btn);
        }
        return result;
    }

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